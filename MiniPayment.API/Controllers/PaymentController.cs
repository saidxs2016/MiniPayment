using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniPayment.Appliaction.Commands;
using MiniPayment.Appliaction.Queries;

namespace MiniPayment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;


        public PaymentController(IMediator mediator) => _mediator = mediator;



        [HttpPost("Sale")]
        public async Task<IActionResult> SaleAsync([FromBody] SaleRequest model) => Ok(await _mediator.Send(model));

        [HttpPost("Cancel")]
        public async Task<IActionResult> CancelAsync([FromBody] CancelRequest model) => Ok(await _mediator.Send(model));

        [HttpPost("Refund")]
        public async Task<IActionResult> RefundAsync([FromBody] RefundRequest model) => Ok(await _mediator.Send(model));




        [HttpPost("FilterBy")]
        public async Task<IActionResult> FilterAsync([FromBody] FilterByRequest model) => Ok(await _mediator.Send(model));


    }
}