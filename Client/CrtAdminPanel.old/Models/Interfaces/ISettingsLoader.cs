using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrtAdminPanel.Models.Classes;

namespace CrtAdminPanel.Models.Interfaces
{
    public interface ISettingsLoader
    {
        Task SaveSettingsAsync(ISettings settings);
        Task<Settings> LoadSettingsAsync();
    }
}
