using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Utils.Logging
{
    public interface ILoggerService
    {
        public void LogInformation(string Message);
        public void LogError(string Message);
        // public void Log
    }
}