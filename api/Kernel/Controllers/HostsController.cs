using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataStructures;

namespace Kernel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ControllerBase
    {
        private readonly ILogger<HostsController> _logger;
        private IHostsStorage _hostsStorage;

        public HostsController(ILogger<HostsController> logger,
                               IHostsStorage hostsStorage)
        {
            _logger = logger;
            _hostsStorage = hostsStorage;
        }

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<IEnumerable<ClientHost>>> GetHosts()
        {
            _logger.LogInformation("Trying to load a list of registered hosts....");
            try
            {
                var hosts = await _hostsStorage.GetClientHosts();

                if (hosts == null)
                {
                    _logger.LogWarning("The database is empty.");
                    return NotFound();
                }

                _logger.LogInformation("The list of hosts has been uploaded successfully.");
                return Ok(hosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateHost(ClientHost host)
        {
            _logger.LogInformation("Trying to update host information with IP = {0}", host.IP);
            try
            {
                await _hostsStorage.UpdateClientHost(host);
                _logger.LogInformation("Information about the host with IP = {0} successfully updated", host.IP);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteHost(ClientHost host)
        {
            _logger.LogInformation("Trying to delete a host with IP = {0}", host.IP);
            try
            {
                await _hostsStorage.DeleteClientHost(host);
                _logger.LogInformation("User with IP = {0} successfully deleted.", host.IP);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
