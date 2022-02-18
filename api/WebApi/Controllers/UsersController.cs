using ElectronicDigitalSignatire.Models.Classes;
using ElectronicDigitalSignatire.Services.Interfaces;
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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private UsersRegistrationServiceCommunicator _usersRegistrationServiceCommunicator;

        public UsersController(ILogger<UsersController> logger,
                               UsersRegistrationServiceCommunicator usersRegistrationServiceCommunicator)
        {
            _logger = logger;
            _usersRegistrationServiceCommunicator = usersRegistrationServiceCommunicator;
        }

        #region -= GET =-

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersFromDbAsync()
        {
            _logger.LogInformation("Loading subjects list from database");
            var users = await _usersRegistrationServiceCommunicator.GetUsersAsync();

            if (users == null)
            {
                _logger.LogInformation("Subjects list is empty");
                return NotFound();
            }

            _logger.LogInformation("Loaded");
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUserByIDAsync(int id)
        {
            var user = await _usersRegistrationServiceCommunicator.GetUserByIDAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        #endregion
    }
}
