using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAndWorkers.Api.Controllers.Requests;
using ProjectsAndWorkers.Api.Controllers.Responses;
using ProjectsAndWorkers.Api.Models;


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
                string lowerSearchText = request.SearchText.ToLower();
				string[] strings = lowerSearchText.Split(' ');

				foreach (var str in strings)
				{
				    query = query.Where(w => 
                    w.FirstName.ToLower().Contains(str) ||
                    w.LastName.ToLower().Contains(str) ||
                    w.Patronymic.ToLower().Contains(str)
                    );					
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
