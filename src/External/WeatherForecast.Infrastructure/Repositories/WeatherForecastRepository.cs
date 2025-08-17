using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Domain.Contracts;
using WeatherForecast.Domain.Entities;
using WeatherForecast.Infrastructure.Data;

namespace WeatherForecast.Infrastructure.Repositories
{
    public class WeatherForecastRepository : BaseRepository<WeatherFC>, IWeatherForecastRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public WeatherForecastRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WeatherFC> GetWeatherByCityAsync(string city)
        {
            return await _dbContext.WeatherForecasts.FirstOrDefaultAsync(w => w.City == city);
        }

    }
}
