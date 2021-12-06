using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Pokemon
{
    public static class StartupExtensions
    {
        public static void AddSeriLogLogging(this ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(@".\log\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            loggerFactory.AddSerilog(logger);
            Log.Logger = logger;
        }

    }
}
