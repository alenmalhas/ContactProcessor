using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactProcessor.Utilities.Logger
{
    public class Logger : ILogger
    {
        public void Log(string message, LogLevel logLevel)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}