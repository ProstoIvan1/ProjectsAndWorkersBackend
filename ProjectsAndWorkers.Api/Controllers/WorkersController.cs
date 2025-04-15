using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectsAndWorkers.Api.Controllers.Requests;
using ProjectsAndWorkers.Api.Controllers.Requests.Workers;
using ProjectsAndWorkers.Api.Controllers.Responses;
using ProjectsAndWorkers.Api.Controllers.Responses.Workers;
using ProjectsAndWorkers.Data;
using ProjectsAndWorkers.Data.Models;
using System.Linq.Expressions;


namespace ProjectsAndWorkers.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
		private ProjectsAndWorkersDataContext _dataContext;

		public WorkersController(ProjectsAndWorkersDataContext dataContext)
		{
            _dataContext = dataContext;
		}

		[HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetWorkersRequest request, CancellationToken ct)
        {
			IQueryable<Worker> query = _dataContext.Workers;

            if(!string.IsNullOrEmpty(request.SearchText))
			{
				string[] strings = request.SearchText.Split(' ');
				foreach (var str in strings)
				{
                    query = query.Where(w =>
                    EF.Functions.Like(w.FirstName + w.LastName + w.Patronymic, $"%{str}%"));
                }
			}

			Worker[] workers = await query.ToArrayAsync(ct);
            GetWorkersResponse response = new GetWorkersResponse(workers.Select(w => w.ToResponse()).ToList());

			return Ok(response);
		}

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken ct)
        {
			Worker? worker = await _dataContext.Workers.FindAsync(id, ct);

            if (worker != null)
                return Ok(worker.ToResponse());

            else return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateWorkerRequest request, CancellationToken ct)
        {
            

            await _dataContext.Workers.AddAsync(request.ToWorker());
            await _dataContext.SaveChangesAsync(ct);

			return Ok();
		}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateWorkerRequest request, CancellationToken ct)
        {
            Worker? worker = await _dataContext.Workers.FindAsync(id, ct);

            if (worker != null)
            {
                worker.Update(request);
                await _dataContext.SaveChangesAsync(ct);
                return Ok();
            }

			else return NotFound("Worker is not found");
		}

        [HttpPut("{performerId}")]
        public async Task<IActionResult> AddTasks([FromRoute] int performerId, [FromBody] AddTasksToWorkerRequest request, CancellationToken ct)
        {
			// check existence of the worker
			if (!await _dataContext.Workers.IsExists(performerId, ct))
				return NotFound("Worker was not found");

			// check the existence of given ids

			int? incorrectId = await _dataContext.Tasks.GetIncorrectId(ct, request.taskIds);

			if (incorrectId != null)
				return NotFound($"Task {incorrectId} was not found");

			// update tasks

			await _dataContext.Tasks
				.Where(t => request.taskIds.Contains(t.Id))
				.ExecuteUpdateAsync(s => s.SetProperty(t => t.PerformerId, performerId), ct);

			return Ok();
		}

		[HttpPut("{performerId}")]
		public async Task<IActionResult> RemoveTasks([FromRoute] int performerId, [FromBody] RemoveTasksFromWorkerRequest request, CancellationToken ct)
		{
			// check existence of the worker
			if (!await _dataContext.Workers.IsExists(performerId, ct))
				return NotFound("Worker was not found");

			// check the existence of given ids
			int? incorrectId = await _dataContext.Tasks.GetIncorrectId(ct, request.taskIds);
			if (incorrectId != null)
				return NotFound($"Task {incorrectId} was not found");

			await _dataContext.Tasks
				.Where(t => request.taskIds.Contains(t.Id) && t.PerformerId == performerId)
				.ExecuteUpdateAsync(s => s.SetProperty(t => t.PerformerId, (int?) null), ct);

			return Ok();
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
			Worker? worker = await _dataContext.Workers
                .Include(w => w.ProjectsNavigation)
                .Include(w => w.ProjectsToManage)
                .Where(w => w.Id == id)
                .FirstOrDefaultAsync(ct);

			if (worker != null)
			{
                // remove all relationships

                worker.ProjectsToManage.Clear();
                worker.ProjectsNavigation.Clear();

                // remove worker

				_dataContext.Workers.Remove(worker);
                await _dataContext.SaveChangesAsync(ct);
				return Ok();
			}
			else return NotFound("Worker is not found");
		}
    }
}
