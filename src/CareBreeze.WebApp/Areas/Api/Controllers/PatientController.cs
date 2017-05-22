using CareBreeze.WebApp.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CareBreeze.WebApp.Areas.Api.Controllers
{
    [Area("Api")]
    public class PatientController : Controller
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Features.Patient.Index.Query query)
        {
            var result = await _mediator.Send(query);
            if (result.Patients.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] Features.Patient.Create.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResult
                {
                    Errors = ModelState.Errors().ToList()
                });
            }
            var result = await _mediator.Send(command);
            if (result.HasError)
            {
                return BadRequest(result);
            }
            return this.CreatedAtArea(nameof(Index), result.PatientId, result);
        }
    }
}
