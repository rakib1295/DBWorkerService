using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWorkerService
{
    public static class AppSettings
    {
        public static IConfiguration Configuration { get; set; }
        public static string OracleConnectionString { get; set; }
        public static string SqlServerConnectionString { get; set; }
        public static string PullRowCount { get; set; }
        public static string WorkingIntervalMinutes { get; set; }
    }
}
