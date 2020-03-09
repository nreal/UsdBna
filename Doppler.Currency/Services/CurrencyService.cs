﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrossCutting;
using Doppler.Currency.Dtos;
using Doppler.Currency.Enums;
using Doppler.Currency.Logger;

namespace Doppler.Currency.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILoggerAdapter<CurrencyService> _logger;
        private readonly IReadOnlyDictionary<CurrencyCodeEnum, CurrencyHandler> _currencyHandlers;

        public CurrencyService(
            ILoggerAdapter<CurrencyService> logger,
            IReadOnlyDictionary<CurrencyCodeEnum, CurrencyHandler> currencyHandlers) =>
            (_logger, _currencyHandlers) = (logger, currencyHandlers);

        public async Task<EntityOperationResult<CurrencyDto>> GetCurrencyByCurrencyCodeAndDate(
            DateTime date,
            CurrencyCodeEnum currencyCode)
        {
            var result = new EntityOperationResult<CurrencyDto>();
            try
            {
                _logger.LogInformation("Service Getting currency code handler.");
                _currencyHandlers.TryGetValue(currencyCode, out var handler);

                if (handler != null)
                    return await handler.Handle(date);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Invalid currency code.");
                result.AddError("Currency code invalid", $"Currency code invalid: {currencyCode}.");
                return result;
            }

            result.AddError("Currency code invalid", $"Currency code invalid: {currencyCode}.");
            return result;
        }
    }
}
