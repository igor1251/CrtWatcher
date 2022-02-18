using HostsRegistrationService.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.GrpcServices;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ControllerBase
    {
        private readonly ILogger<HostsController> _logger;
        private HostsRegistrationServiceCommunicator _hostsRegistrationServiceCommunicator;

        public HostsController(ILogger<HostsController> logger,
                               HostsRegistrationServiceCommunicator hostsRegistrationServiceCommunicator)
        {
            _logger = logger;
            _hostsRegistrationServiceCommunicator = hostsRegistrationServiceCommunicator;
        }

        #region -= GET =-

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<IEnumerable<IClientHost>>> GetClientHostsAsync()
        {
            var hosts = await _hostsRegistrationServiceCommunicator.GetRegisteredClientHostsAsync();
            if (hosts == null) return NotFound();
            return Ok(hosts);
        }

        #endregion
    }
}
