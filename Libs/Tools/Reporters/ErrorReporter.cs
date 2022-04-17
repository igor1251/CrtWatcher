using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Reporters
{
    public static class ErrorReporter
    {
        private const string logfile = "error.log";

        public static async Task MakeReport(string moduleName, Exception ex)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine(DateTime.Now.ToString());
            report.AppendLine("\tError in module: " + moduleName);
            report.AppendLine("\tException message is: \"" + ex.Message + "\"");
            await File.AppendAllTextAsync(logfile, report.ToString());
        }
    }
}
