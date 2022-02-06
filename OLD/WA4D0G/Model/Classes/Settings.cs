using System;
using WA4D0G.Model.Interfaces;

namespace WA4D0G.Model.Classes
{
    public class Settings : ISettings
    {
        private bool _personalKeyStore;

        private uint _warnDaysCount;
        private string _dbFileName = "keys.sqlite";

        public bool PersonalKeyStore 
        { 
            get => _personalKeyStore; 
            set => _personalKeyStore = value; 
        }

        public uint WarnDaysCount
        {
            get => _warnDaysCount;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("WarnDaysCount must be above 0. You trying to set it to 0 or lower.");
                }

                _warnDaysCount = value;
            }
        }

        public string DbFileName 
        { 
            get => _dbFileName; 
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("DbFileName must be a string, not null or empty string.");
                }

                _dbFileName = value;
            }
        }
        
        public string BaseDirectory 
        { 
            get => Environment.CurrentDirectory + "\\Db\\"; 
        }
        
        public string DbPath 
        { 
            get => BaseDirectory + DbFileName; 
        }
    }
}
