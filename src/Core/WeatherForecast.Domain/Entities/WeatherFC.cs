using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Domain.Entities
{
    public class WeatherFC
    {
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public double TemperatureCelsius { get; set; }
        public string? Conditions { get; set; }
        public double HumidityPercentage { get; set; }
        public double WindSpeedKmh { get; set; }

        public double TemperatureFahrenheit
        {
            get { return TemperatureCelsius * 9 / 5 + 32; }
        }
    }
}
