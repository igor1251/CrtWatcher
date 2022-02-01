using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ElectrnicDigitalSignatire.Services.Interfaces;

namespace WA4D0GServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly ILogger<CertificatesController> _logger;
        private IDbStore _dbStore;

        public CertificatesController(ILogger<CertificatesController> logger,
                                      IDbStore dbStore)
        {
            _logger = logger;
            _dbStore = dbStore;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCertificate(int id)
        {
            _logger.LogInformation("Deliting certificate under id=" + id);
            await _dbStore.DeleteCertificate(id);
            _logger.LogInformation("Done");
            return NoContent();
        }
    }
}
