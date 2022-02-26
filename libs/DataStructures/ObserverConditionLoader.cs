using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStructures
{
    public class ObserverConditionLoader
    {
        private readonly string _conditionConfigFile = Environment.CurrentDirectory + "\\observerCondition.cfg",
                                _errorLogPath = Environment.CurrentDirectory + "\\error.log";

        public async Task<ObserverCondition> LoadObserverConditionAsync()
        {
            try
            {
                FileStream fs = new FileStream(_conditionConfigFile, FileMode.OpenOrCreate, FileAccess.Read);
                if (fs.Length == 0)
                {
                    fs.Close();
                    return ObserverCondition.FirstLaunch;
                }
                var observerCondition = await JsonSerializer.DeserializeAsync<ObserverCondition>(fs);
                fs.Close();
                return observerCondition;
            }
            catch (Exception ex)
            {
                await File.AppendAllTextAsync(_errorLogPath, DateTime.Now.ToString() + " " + ex.Message);
                return ObserverCondition.Error;
            }
        }

        public async Task SaveObserverConditionAsync(ObserverCondition observerCondition)
        {
            try
            {
                FileStream fs = new FileStream(_conditionConfigFile, FileMode.OpenOrCreate, FileAccess.Write);
                await JsonSerializer.SerializeAsync(fs, observerCondition);
                fs.Close();
            }
            catch (IOException ex)
            {
                await File.AppendAllTextAsync(_errorLogPath, DateTime.Now.ToString() + " " + ex.Message);
            }
        }
    }
}
