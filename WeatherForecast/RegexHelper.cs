using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherForecast
{
    public static class RegexHelper
    {
        public static string ImageSource(string text)
        {
            Regex regexImageSource = new Regex(@"dist\/images\/v2\/svg\/\w+\.svg");
            return regexImageSource.Match(text).Value;
        }

        public static List<Match> ListImageSource(string text)
        {
            Regex regexImageSource = new Regex(@"dist\/images\/v2\/svg\/\w+\.svg");
            return regexImageSource.Matches(text).ToList();
        }
    }
}
