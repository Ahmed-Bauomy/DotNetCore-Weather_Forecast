using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Application.Dtos
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public long ExpiresIn { get; set; }
        public string UserName { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }
}
