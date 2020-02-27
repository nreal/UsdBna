﻿using System;
using System.Threading.Tasks;
using Doppler.Currency.Dtos;
using Doppler.Currency.Logger;
using Doppler.Currency.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Doppler.Currency.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsdCurrencyController : ControllerBase
    {
        private readonly ILoggerAdapter<UsdCurrencyController> _logger;
        private readonly ICurrencyService _currencyService;

        public UsdCurrencyController(ILoggerAdapter<UsdCurrencyController> logger, ICurrencyService currencyService) => 
            (_logger, _currencyService) = (logger, currencyService);

        [HttpGet("{countryCode}/{date}")]
        [SwaggerOperation(Summary = "Get currency by country and date")]
        [SwaggerResponse(200, "The currency is ok", typeof(UsdCurrency))]
        [SwaggerResponse(400, "The currency data is invalid")]
        public async Task<IActionResult> Get(
            [SwaggerParameter(Description = "MM-dd-YYYY")] DateTime date,
            [SwaggerParameter(Description = "ARG, MEX")] string countryCode)
        {
            _logger.LogInformation("Getting Usd today.");
            var result = await _currencyService.GetUsdTodayByCountry(date, countryCode);

            if (result.Success)
                return Ok(result);

            return BadRequest(result.Errors);
        }
    }
}
