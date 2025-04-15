using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAndWorkers.Api.Controllers.Filtering;
using ProjectsAndWorkers.Api.Controllers.Filtering.Projects;
using ProjectsAndWorkers.Api.Controllers.Requests;
using ProjectsAndWorkers.Api.Controllers.Requests.Projects;
using ProjectsAndWorkers.Api.Controllers.Responses;
using ProjectsAndWorkers.Api.Controllers.Responses.Projects;
using ProjectsAndWorkers.Api.Controllers.Sorting;
using ProjectsAndWorkers.Api.Controllers.Sorting.Projects;
using ProjectsAndWorkers.Data;
using ProjectsAndWorkers.Data.Models;

namespace ProjectsAndWorkers.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private ProjectsAndWorkersDataContext _dataContext;

		public ProjectsController(ProjectsAndWorkersDataContext dataContext)
		{
            _dataContext = dataContext;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProjectsRequest request)
		{
			var (sortItemName, sortDirection, minPriority, maxPriority, startDateRange, endDateRange) = request;

			IQueryable<Project> query = _dataContext.Projects;

			// filters

			FilterManager<Project> filterManager = new(
				new ProjectMaxPriorityFilter(maxPriority),
				new ProjectMinPriorityFilter(minPriority),
				new StartDateRangeFilter(startDateRange),
				new EndDateRangeFilter(endDateRange));

			filterManager.Filter(ref query);

			// sorting by id, priority, start and end dates
			
			var sorter = new Sorter<Project>(
				new ProjectNameProperty(), 
				new EndDateProperty(), 
				new StartDateProperty(), 
				new ProjectPriorityProperty());

			bool isDesc = sortDirection == "desc";

			sorter.Sort(ref query, sortItemName, isDesc);

			// get projects

			Project[] projects = await query.ToArrayAsync();
			List<GetProjectResponse> records = new();

			foreach (var project in projects)
			{
				records.Add(project.ToResponse());
			}

			return Ok(new GetProjectsResponse(records));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken ct)
        {
            Project? project = await _dataContext.Projects.FindAsync(id, ct);

            if (project != null)
                return Ok(project.ToResponse());

            else return NotFound("Project was not found");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProjectRequest request, CancellationToken ct)
        {
			// check dates

			if (request.StartDate >= request.EndDate)
				return Conflict($"Start date should be less than end date");

			// check the existence of given ids

			int? incorrectId = await _dataContext.Workers.GetIncorrectId(ct, [.. request.WorkerIds, request.ManagerId]);

			if (incorrectId != null)
				return NotFound($"Worker {incorrectId} is not found");

			// create a project

            Project project = request.ToProject();

			_dataContext.Workers.AttachRange(project.Workers);

            await _dataContext.Projects.AddAsync(project, ct);

			await _dataContext.SaveChangesAsync(ct);

            return Ok();
		}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateProjectRequest request, CancellationToken ct)
        {
			// check the existence of manager

			bool isManagerExists = await _dataContext.Workers.GetIncorrectId(ct, request.ManagerId) == null;
			if (!isManagerExists)
				return NotFound("Manager is not found");

			// update project

			Project? project = await _dataContext.Projects.FindAsync(id, ct);

			if (project != null)
			{
				project.Update(request);
				await _dataContext.SaveChangesAsync(ct);

				return Ok();
			}

			else return NotFound("Project was not found");
		}

		[HttpPut("{projectId}")]
		public async Task<IActionResult> AddWorkers([FromRoute] int projectId, AddWorkersToProjectRequest request, CancellationToken ct)
		{
			// get project from database including workers

			Project? project = await _dataContext.Projects
				.Include(p => p.Workers)
				.Where(p => p.Id == projectId)
				.FirstOrDefaultAsync(ct);

			if (project == null)
				return NotFound("Project was not found");

			// check the existence of given ids

			int? incorrectId = await _dataContext.Workers.GetIncorrectId(ct, request.workerIds);

			if (incorrectId != null)
				return NotFound($"Worker {incorrectId} was not found");

			// add workers by id

            foreach (var workerId in request.workerIds)
			{
                bool isAdded = project.Workers.Where(w => w.Id == workerId).Any();
                if (isAdded)
                    return Conflict($"Worker {workerId} has already been added.");

                project.Workers.Add(new Worker() { Id = workerId });
			}


			_dataContext.Workers.AttachRange(project.Workers);

			await _dataContext.SaveChangesAsync(ct);

			return Ok();
		}

		[HttpPut("{projectId}")]
		public async Task<IActionResult> RemoveWorkers([FromRoute] int projectId, [FromBody] RemoveWorkersFromProjectRequest request, CancellationToken ct)
        {
			// get project from database including workers

			Project? project = await _dataContext.Projects
				.Include(p => p.Workers)
				.Where(p => p.Id == projectId)
				.FirstOrDefaultAsync(ct);

			if (project == null)
				return NotFound("Project was not found");

			_dataContext.Workers.AttachRange(project.Workers);

			// remove workers by id

			foreach (var workerId in request.workerIds)
			{
				Worker? worker = project.Workers.Where(w => w.Id == workerId).FirstOrDefault();
				if (worker != null)
					project.Workers.Remove(worker);
				else return NotFound($"Worker {workerId} was not found");
			}

			await _dataContext.SaveChangesAsync(ct);

			return Ok();
		}

		[HttpPut("{projectId}")]
		public async Task<IActionResult> AddTasks([FromRoute] int projectId, [FromBody] AddTasksToProjectRequest request, CancellationToken ct)
		{
			// check existence of the project

			if (!await _dataContext.Projects.IsExists(projectId, ct))
				return NotFound("Project was not found");

			// check the existence of given ids

			int? incorrectId = await _dataContext.Tasks.GetIncorrectId(ct, request.taskIds);

			if (incorrectId != null)
				return NotFound($"Task {incorrectId} was not found");

			// update tasks

			await _dataContext.Tasks
				.Where(t => request.taskIds.Contains(t.Id))
				.ExecuteUpdateAsync(s => s.SetProperty(t => t.ProjectId, projectId), ct);

			return Ok();
		}

		[HttpPut("{projectId}")]
		public async Task<IActionResult> RemoveTasks([FromRoute] int projectId, [FromBody] AddTasksToProjectRequest request, CancellationToken ct)
		{
			// check existence of the project

			if (!await _dataContext.Projects.IsExists(projectId, ct))
				return NotFound("Project was not found");

			// check the existence of given ids

			int? incorrectId = await _dataContext.Tasks.GetIncorrectId(ct, request.taskIds);
			if (incorrectId != null)
				return NotFound($"Task {incorrectId} was not found");

			// delete tasks

			await _dataContext.Tasks
				.Where(t => request.taskIds.Contains(t.Id) && t.ProjectId == projectId)
				.ExecuteDeleteAsync();

			return Ok();
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            Project? project = await _dataContext.Projects
				.Include(p => p.Workers)
				.Where(p => p.Id == id)
				.FirstOrDefaultAsync(ct);

            if (project != null)
            {
				project.Workers.Clear();

                _dataContext.Projects.Remove(project);
                await _dataContext.SaveChangesAsync(ct);
                return Ok();
            }

            else return NotFound("Project was not found");
		}

		
    }
}
