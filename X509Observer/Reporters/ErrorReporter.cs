using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using X509Observer.MagicStrings.MaintananceFilesNames;

namespace X509Observer.Reporters
{
    public static class ErrorReporter
    {
        public static async Task MakeReport(string moduleName, Exception ex)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine(DateTime.Now.ToString());
            report.AppendLine("\tError in module: " + moduleName);
            report.AppendLine("\tException message is: \"" + ex.Message + "\"");
            await File.AppendAllTextAsync(FileNames.ERROR_LOG_FILE_NAME, report.ToString());
        }
    }
}
