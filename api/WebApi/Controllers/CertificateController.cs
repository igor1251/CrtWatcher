using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ElectronicDigitalSignatire.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly ILogger<CertificateController> _logger;

        public CertificateController(ILogger<CertificateController> logger)
        {
            _logger = logger;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCertificate(int id)
        {
            _logger.LogInformation("Deliting certificate under id=" + id);

            //

            _logger.LogInformation("Done");
            return NoContent();
        }
    }
}
