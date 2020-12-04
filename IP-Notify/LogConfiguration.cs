using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Notify
{
    static class LogConfiguration
    {
        public static Serilog.Core.Logger Setup(string fileName)
        {
            return new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.Console()
                .WriteTo.File(fileName, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
