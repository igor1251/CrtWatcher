using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DataStructures;

namespace Kernel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;
        private ISettingsStorage _settingsStorage;

        public SettingsController(ILogger<SettingsController> logger,
                                  ISettingsStorage settingsStorage)
        {
            _logger = logger;
            _settingsStorage = settingsStorage;
        }

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<Settings>> GetSettings()
        {
            _logger.LogInformation("Trying to load information about the settings....");
            try
            {
                var settings = await _settingsStorage.LoadSettings();
                _logger.LogInformation("Settings loaded successfully.");
                return Ok(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSettings(Settings settings)
        {
            _logger.LogInformation("Trying to update the settings....");
            try
            {
                await _settingsStorage.UpdateSettings(settings);
                _logger.LogInformation("Settings have been updated successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
