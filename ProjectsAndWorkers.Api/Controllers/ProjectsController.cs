using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAndWorkers.Api.Controllers.Filtering;
using ProjectsAndWorkers.Api.Controllers.Requests;
using ProjectsAndWorkers.Api.Controllers.Responses;
using ProjectsAndWorkers.Api.Controllers.Sorting;
using ProjectsAndWorkers.Api.Models;
using System.Linq.Expressions;
using System.Net.Mail;

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

			FilterManager<Project> filters = new([]);

			if (maxPriority != null)
				filters.Filters.Add(new MaxPriorityFilter((int) maxPriority));

			if (minPriority != null)
				filters.Filters.Add(new MinPriorityFilter((int) minPriority));

			if (startDateRange != null)
				filters.Filters.Add(new StartDateRangeFilter((DateOnly)startDateRange));

			if (endDateRange != null)
				filters.Filters.Add(new EndDateRangeFilter((DateOnly)endDateRange));

			filters.Filter(ref query);

			// sorting by id, priority, start and end dates 
			var sorter = new Sorter<Project>(new NameProperty(), new EndDateProperty(), new StartDateProperty(), new PriorityProperty());

			bool isDesc = sortDirection == "desc";

			sorter.Sort(ref query, sortItemName, isDesc);

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

			int? incorrectId = await _dataContext.GetIncorrectWorkerId([.. request.WorkerIds, request.ManagerId]);

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

			bool isManagerExists = await _dataContext.GetIncorrectWorkerId(request.ManagerId) == null;
			if (!isManagerExists)
				return NotFound("Manager is not found");

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

			int? incorrectId = await _dataContext.GetIncorrectWorkerId(request.workerIds);

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
		public async Task<IActionResult> RemoveWorker([FromRoute] int projectId, [FromBody] RemoveWorkersFromProjectRequest request, CancellationToken ct)
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
