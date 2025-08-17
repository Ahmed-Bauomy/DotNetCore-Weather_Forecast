using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.Infrastructure.Data.Configurations
{
    public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherFC>
    {
        public void Configure(EntityTypeBuilder<WeatherFC> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Date)
                       .IsRequired();

            builder.Property(p => p.TemperatureCelsius)
                       .IsRequired();

            builder.Property(p => p.Conditions)
                       .IsRequired();

            builder.Property(p => p.HumidityPercentage)
                       .IsRequired();

            builder.Property(p => p.WindSpeedKmh)
                       .IsRequired();
            
        }
    }
}
