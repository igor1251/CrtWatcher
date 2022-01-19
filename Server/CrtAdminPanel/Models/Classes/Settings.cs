using System;
using System.ComponentModel.DataAnnotations;
using CrtAdminPanel.Models.Interfaces;

namespace CrtAdminPanel.Models.Classes
{
    public class Settings : ISettings
    {
        private bool _personalKeyStore = false;

        private uint _warnDaysCount;
        private string _dbFileName = "keys.sqlite";

        [Required]
        public bool PersonalKeyStore 
        { 
            get => _personalKeyStore; 
            set => _personalKeyStore = value; 
        }

        [Required]
        [Range(0, 365, ErrorMessage = "The number of days should be in the range from 0 to 365")]
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

        [Required]
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

        [Required]
        public string BaseDirectory 
        { 
            get => Environment.CurrentDirectory + "\\Db\\"; 
        }

        [Required]
        public string DbPath 
        { 
            get => BaseDirectory + DbFileName; 
        }
    }
}
