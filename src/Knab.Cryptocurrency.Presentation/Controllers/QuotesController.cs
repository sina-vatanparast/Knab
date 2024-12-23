using Knab.Cryptocurrency.Application.Queries;
using Knab.Cryptocurrency.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Knab.Cryptocurrency.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController(IMediator mediator, IOptions<AppSettings> appSettings) : ControllerBase
    {
        [HttpGet("{name}/{slug}")]
        public async Task<IActionResult> GetAll(string name, string slug)
        {
            var quotes = await mediator.Send(new GetQuotesQuery 
            {
                BaseCurrencyName = name,
                BaseCurrencySlug = slug,
                DefaultFiatCurrencyCode = appSettings.Value.DefaultFiatCurrencyCode,
                QuoteCurrencyCodes = appSettings.Value.FiatCurrencyCodes,

            });

            if (quotes?.Any() != true)
            {
                return NotFound();
            }

            return Ok(quotes);
        }
    }
}
