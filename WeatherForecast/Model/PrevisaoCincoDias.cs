using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Model
{
    public struct PrevisaoCincoDias
    {
        public string Dia { get; set; }
        public string DiaSemana { get; set; }
        public int Minima { get; set; }
        public int Maxima { get; set; }
        public int ProbabilidadeChuva { get; set; }
        public string Clima { get; set; }
        public int Precipitacao { get; set; }
        public int UmidadeMinima { get; set; }
        public int UmidadeMaxima { get; set; }
    }
}
