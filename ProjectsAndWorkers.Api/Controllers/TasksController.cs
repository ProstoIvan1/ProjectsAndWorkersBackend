using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAndWorkers.Api.Controllers.Filtering;
using ProjectsAndWorkers.Api.Controllers.Filtering.Tasks;
using ProjectsAndWorkers.Api.Controllers.Requests;
using ProjectsAndWorkers.Api.Controllers.Requests.Tasks;
using ProjectsAndWorkers.Api.Controllers.Responses;
using ProjectsAndWorkers.Api.Controllers.Responses.Tasks;
using ProjectsAndWorkers.Api.Controllers.Sorting;
using ProjectsAndWorkers.Api.Controllers.Sorting.Tasks;
using ProjectsAndWorkers.Data;
using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        ProjectsAndWorkersDataContext _dataContext;

		public TasksController(ProjectsAndWorkersDataContext dataContext)
		{
			_dataContext = dataContext;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTasksRequest request, CancellationToken ct)
        {
			var (sortItem, sortDirection, maxPriority, minPriority, status, authorId, performerId, projectId) = request;
			sortItem = sortItem?.ToLower();
			sortDirection = sortDirection?.ToLower();

			IQueryable<TaskEntity> query = _dataContext.Tasks;

			// sort

			Sorter<TaskEntity> sorter = new(new TaskTitleProperty(), new TaskStatusProperty(), new TaskPriorityProperty());

			sorter.Sort(ref query, sortItem, sortDirection == "desc");

			// filters

			FilterManager<TaskEntity> manager = new(
				new TaskMaxPriorityFilter(maxPriority), 
				new TaskMinPriorityFilter(minPriority), 
				new StatusFilter(status),
				new AuthorIdFilter(authorId),
				new PerformerIdFilter(performerId),
				new ProjectIdFilter(projectId));

			manager.Filter(ref query);

			// get response

            TaskEntity[] tasks = await query.ToArrayAsync(ct);

            GetTasksResponse response = new(new List<GetTaskResponse>());

			foreach (var task in tasks)
			{
				response.Tasks.Add(task.ToResponse());
			}

			return Ok(response);
		}

		[Route("{id}")]
		[HttpGet]
		public async Task<IActionResult> Get([FromRoute] int id, CancellationToken ct)
		{
			TaskEntity? task = await _dataContext.Tasks.FindAsync(id, ct);

			if (task != null)
			{
				return Ok(task.ToResponse());
			}

			else return NotFound("Task was not found");
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateTaskRequest request, CancellationToken ct)
		{
			// check project

			bool isProjectExists = await _dataContext.Projects.IsExists(request.ProjectId, ct);

			if (!isProjectExists)
				return NotFound("Project was not found");

			// check author and performer

			int? incorrectId;
			
			if (request.PerformerId != null)
				incorrectId = await _dataContext.Workers.GetIncorrectId(ct, request.AuthorId, (int) request.PerformerId);
			else
				incorrectId = await _dataContext.Workers.GetIncorrectId(ct, request.AuthorId);

			if (incorrectId != null)
				return NotFound($"Worker {incorrectId} was not found");
			
			// creating tasks

			TaskEntity task = request.ToTask();

			await _dataContext.Tasks.AddAsync(task, ct);

			await _dataContext.SaveChangesAsync(ct);

			return Ok();
		}

		[Route("{id}")]
		[HttpPut]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskRequest request, CancellationToken ct)
		{
			if (await _dataContext.Tasks.IsExists(id, ct))
			{
				var (title, description, status, priority, authorId, performerId, projectId) = request;

				await _dataContext.Tasks.Where(t => t.Id == id).ExecuteUpdateAsync(s => s
					.SetProperty(t => t.Title, title)
					.SetProperty(t => t.Description, description)
					.SetProperty(t => t.Status, status)
					.SetProperty(t => t.Priority, priority)
					.SetProperty(t => t.AuthorId, authorId)
					.SetProperty(t => t.PerformerId, performerId)
					.SetProperty(t => t.ProjectId, projectId),
					
					ct);

				return Ok();
			}

			else return NotFound("Task was not found");
		}

		[Route("{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
		{
			if(await _dataContext.Tasks.IsExists(id, ct))
			{
				await _dataContext.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync(ct);
				return Ok();
			}

			return NotFound("Task was not found");
		}


    }
}
