using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DataStructures;
using System;

namespace Kernel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly ILogger<CertificateController> _logger;
        private IUsersStorage _usersStorage;

        public CertificateController(ILogger<CertificateController> logger,
                                     IUsersStorage usersStorage)
        {
            _logger = logger;
            _usersStorage = usersStorage;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCertificate(int id)
        {
            _logger.LogInformation("Trying to delete a certificate with ID = {0}", id);
            try
            {
                await _usersStorage.DeleteCertificate(id);
                _logger.LogInformation("Certificate with ID = {0} successfully deleted.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

            return NoContent();
        }
    }
}
