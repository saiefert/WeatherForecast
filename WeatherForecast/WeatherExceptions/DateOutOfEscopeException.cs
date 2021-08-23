using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimaRmc.ClimaExceptions
{
    public class DateOutOfEscopeException : Exception
    {
        public DateOutOfEscopeException(string message) : base(message)
        {
        }

        public DateOutOfEscopeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
