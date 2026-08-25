using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Command;
using TradingEngine.Extensions;

namespace TradingEngine.Controllers
{
    [ApiController]
    [Route("api/v1/balance")]
    public class BalanceController : ControllerBase
    {
        private readonly ISender _sender;
        public BalanceController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost]
        public async Task<IActionResult> CreateBalance([FromBody] CreateBalanceCommand request)
        {
            var result = await _sender.Send(request);
            if (result.IsSuccess)
            {
                return this.ToCreatedResponse(result);
            }
            return this.ToErrorResponse(result);
        }
    }
}
