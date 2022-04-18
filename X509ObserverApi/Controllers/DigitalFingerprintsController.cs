using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using X509ObserverApi.Attributes;
using X509KeysVault.Repositories;
using Tools.Reporters;

namespace X509ObserverApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DigitalFingerprintsController : ControllerBase
    {
        private readonly ILogger<DigitalFingerprintsController> _logger;
        private ISubjectsRepository _subjectsRepository;

        public DigitalFingerprintsController(ILogger<DigitalFingerprintsController> logger,
                                             ISubjectsRepository subjectsRepository)
        {
            _logger = logger;
            _subjectsRepository = subjectsRepository;
        }

        [Authorization("administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDigitalFingerprint(int id)
        {
            try
            {
                await _subjectsRepository.RemoveDigitalFingerptintByIDAsync(id);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("DeleteDigitalFingerprint(int id)", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }

            return NoContent();
        }
    }
}
