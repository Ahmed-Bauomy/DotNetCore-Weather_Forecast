using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Application.Contracts
{
    public interface IValidationBehaviour<TEntity> where TEntity : class
    {
        Task Validate(TEntity entity);
    }
}
