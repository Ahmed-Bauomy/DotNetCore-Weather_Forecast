using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Application.Dtos
{
    public class WeatherForecastDto
    {
        public Guid Id { get; set; }  
        public DateTime Date { get; set; }
        public double TemperatureCelsius { get; set; }
        public string? Conditions { get; set; }
        public double HumidityPercentage { get; set; }
        public double WindSpeedKmh { get; set; }
        public string City { get; set; }
    }
}
