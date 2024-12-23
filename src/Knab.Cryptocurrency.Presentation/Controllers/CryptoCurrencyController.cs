using Knab.Cryptocurrency.Application.Queries;
using Knab.Cryptocurrency.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoCurrencyController(IMediator mediator, IOptions<AppSettings> appSettings) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cryptoCurrencies = await mediator.Send(new GetCryptoCurrenciesQuery { Limit = appSettings.Value.CryptoCurrenciesLimit });
           
            if (cryptoCurrencies?.Any() != true)
            {
                return NotFound();
            }

            return Ok(cryptoCurrencies);
        }
    }
}
