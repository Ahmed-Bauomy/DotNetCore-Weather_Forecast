using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Contracts;
using WeatherForecast.Application.Exceptions;

namespace WeatherForecast.Application.Behaviours
{
    public class ValidationBehaviour<TEntity> : IValidationBehaviour<TEntity> where TEntity : class
    {
        private readonly IEnumerable<IValidator<TEntity>> validators;

        public ValidationBehaviour(IEnumerable<IValidator<TEntity>> validators)
        {
            this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task Validate(TEntity entity)
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TEntity>(entity);
                var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(r => r != null).ToList();
                if (failures.Count > 0)
                    throw new CustomValidationException(failures);
            }
        }
    }
}
