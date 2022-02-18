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
            _logger.LogInformation("Trying to load a list of registered users....");
            try
            {
                var users = await _usersRegistrationServiceCommunicator.GetUsersAsync();

                if (users == null)
                {
                    _logger.LogWarning("The database is empty.");
                    return NotFound();
                }

                _logger.LogInformation("The list of users has been uploaded successfully.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUserByIDAsync(int id)
        {
            _logger.LogInformation("Trying to download user information with ID = {0}....", id);
            try
            {
                var user = await _usersRegistrationServiceCommunicator.GetUserByIDAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("The user with the specified ID does not exist.");
                    return NotFound();
                }
                _logger.LogInformation("The user with the specified ID was found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        #endregion

        #region -= PUT =-

        [HttpPut]
        public async Task<ActionResult> UpdateUserAsync(User user)
        {
            _logger.LogInformation("Trying to update user information with ID = {0}", user.ID);
            try
            {
                await _usersRegistrationServiceCommunicator.UpdateUserAsync(user);
                _logger.LogInformation("Information about the user with ID = {0} successfully updated", user.ID);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        #endregion

        #region -= DELETE =-

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            _logger.LogInformation("Trying to delete a user with ID = {0}", id);
            try
            {
                var user = await _usersRegistrationServiceCommunicator.GetUserByIDAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("The user with the specified ID does not exist.");
                    return NotFound();
                }
                await _usersRegistrationServiceCommunicator.UnregisterUserAsync(user);
                _logger.LogInformation("User with ID = {0} successfully deleted.", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        #endregion
    }
}
