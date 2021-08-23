using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimaRmc
{
    public static class ImageWeather
    {
        public static string Weather(string imagem)
        {
            var arrayNomes = imagem.Split(@"/");
            var nome = arrayNomes[arrayNomes.Count() - 1].Replace(".svg", "");

            switch (nome)
            {
                case "1":
                case "1n":
                    return "clear";
                case "2":
                case "2n":
                    return "few clouds";
                case "2r":
                case "2rn":
                case "3tm":
                    return "cloudy";
                case "3":
                case "3n":
                    return "light rain";
                case "4":
                case "4n":
                    return "sun with rain";
                case "4r":
                case "4rn":
                    return "rain showers";
                case "4t":
                case "4tn":
                    return "sun rain with thunderstorms";
                case "5":
                case "5n":
                    return "rainy";
                case "6":
                case "6n":
                    return "thunderstorm rain";
                case "7":
                case "7n":
                    return "frost";
                default:
                    return imagem;
            }
        }

    }
}
