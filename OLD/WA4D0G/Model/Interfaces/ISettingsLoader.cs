using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WA4D0G.Model.Classes;

namespace WA4D0G.Model.Interfaces
{
    public interface ISettingsLoader
    {
        Task SaveSettingsAsync(ISettings settings);
        Task<Settings> LoadSettingsAsync();
    }
}
