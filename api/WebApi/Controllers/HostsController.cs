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



        #endregion
    }
}
