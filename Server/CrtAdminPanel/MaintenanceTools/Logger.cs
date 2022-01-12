using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WA4D0G.MaintenanceTools
{
    public static class Logger
    {
        private readonly static string logPath = Environment.CurrentDirectory + "\\Log\\log.txt";

        public static async Task WriteAsync(string message)
        {
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                await writer.WriteLineAsync(DateTime.Now.ToString() + ": " + message).ConfigureAwait(false);
            }
            
            /*
            byte[] data = Encoding.Default.GetBytes(DateTime.Now.ToString() + ": " + text + "\n");

            using (FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await fs.WriteAsync(data, 0, data.Length);
            }
            */
        }
    }
}