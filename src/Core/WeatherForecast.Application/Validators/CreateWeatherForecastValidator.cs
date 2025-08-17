using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Dtos;

namespace WeatherForecast.Application.Validators
{
    public class CreateWeatherForecastValidator : AbstractValidator<CreateWeatherForecastDto>
    {
        public CreateWeatherForecastValidator()
        {

            RuleFor(p => p.Date)
                        .NotEmpty().WithMessage("{Date} is required")
                        .NotNull();

            RuleFor(p => p.TemperatureCelsius)
                        .NotEmpty().WithMessage("{TemperatureCelsius} is required")
                        .NotNull();

            RuleFor(p => p.Conditions)
                        .NotEmpty().WithMessage("{Conditions} is required")
                        .NotNull();

            RuleFor(p => p.HumidityPercentage)
                        .NotEmpty().WithMessage("{HumidityPercentage} is required")
                        .NotNull();

            RuleFor(p => p.WindSpeedKmh)
                        .NotEmpty().WithMessage("{WindSpeedKmh} is required")
                        .NotNull();
        }
    }
}
