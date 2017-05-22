using CareBreeze.WebApp.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CareBreeze.WebApp.Areas.Api.Controllers
{
    [Area("Api")]
    public class ConsultationController : Controller
    {
        private readonly IMediator _mediator;

        public ConsultationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(Features.Consultation.Index.Query query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResult
                {
                    Errors = ModelState.Errors().ToList()
                });
            }
            var result = await _mediator.Send(query);
            if (result.HasError)
            {
                return Json(result);
            }
            return Ok(result);
        }
    }
}
