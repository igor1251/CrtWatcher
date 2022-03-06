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
    public class UsersController : ControllerBase
    {
        private ILogger<UsersController> _logger;
        private IUsersStorage _usersStorage;

        public UsersController(ILogger<UsersController> logger,
                               IUsersStorage usersStorage)
        {
            _logger = logger;
            _usersStorage = usersStorage;
        }

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            _logger.LogInformation("Trying to load a list of registered users....");
            try
            {
                var users = await _usersStorage.GetUsers();

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
        public async Task<ActionResult<User>> GetUserByID(int id)
        {
            _logger.LogInformation("Trying to download user information with ID = {0}....", id);
            try
            {
                var user = await _usersStorage.GetUserByID(id);
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

        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {
            _logger.LogInformation("Trying to register new user....\nusername: {0}\nphone: {1}\ncomment: {2}", user.UserName, user.UserPhone, user.UserComment);
            try
            {
                await _usersStorage.InsertUser(user);
                _logger.LogInformation("The user has been successfully registered.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(User user)
        {
            _logger.LogInformation("Trying to update user information with ID = {0}", user.ID);
            try
            {
                await _usersStorage.UpdateUser(user);
                _logger.LogInformation("Information about the user with ID = {0} successfully updated", user.ID);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Trying to delete a user with ID = {0}", id);
            try
            {
                var user = await _usersStorage.GetUserByID(id);
                if (user == null)
                {
                    _logger.LogWarning("The user with the specified ID does not exist.");
                    return NotFound();
                }
                await _usersStorage.DeleteUser(id);
                _logger.LogInformation("User with ID = {0} successfully deleted.", id);
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
