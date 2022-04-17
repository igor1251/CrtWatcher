using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Tools.Reporters;

namespace NetworkOperators.Identity.MaintananceTools
{
    public static class SHA2HashOperator
    {
        public static async Task<string> Generate(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            using (SHA256 sha256 = SHA256.Create())
            {
                try
                {
                    byte[] rawHash = sha256.ComputeHash(data);
                    return Convert.ToHexString(rawHash);
                }
                catch (Exception ex)
                {
                    await ErrorReporter.MakeReport("Generate(string password)", ex);
                }
                return string.Empty;
            }
        }
    }
}
