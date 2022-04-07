using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X509Observer.DatabaseOperators.Network;
using X509Observer.Primitives.Network;
using X509Observer.Reporters;

namespace Kernel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientDesktopsController : ControllerBase
    {
        private readonly ILogger<ClientDesktopsController> _logger;
        private IClientDesktopsRepository _clientDesktopsRepository;

        public ClientDesktopsController(ILogger<ClientDesktopsController> logger,
                                        IClientDesktopsRepository clientDesktopsRepository)
        {
            _logger = logger;
            _clientDesktopsRepository = clientDesktopsRepository;
        }

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<List<ClientDesktop>>> GetClientDesktops()
        {
            try
            {
                var clientDesktops = await _clientDesktopsRepository.GetClientDesktopsAsync();
                if (clientDesktops == null)
                {
                    return NotFound();
                }
                return Ok(clientDesktops);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetClientDesktops()", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddClientDesktop(ClientDesktop clientDesktop)
        {
            try
            {
                await _clientDesktopsRepository.AddClientDesktopAsync(clientDesktop);
                return Ok();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("AddClientDesktop(ClientDesktop clientDesktop)", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        //[HttpPut]
        //public async Task<ActionResult> UpdateClientDesktop(ClientDesktop clientDesktop)
        //{
        //    _logger.LogInformation("Trying to update host information with IP = {0}", clientDesktop.IP);
        //    try
        //    {
        //        await _clientDesktopsRepository.UpdateClientDesktopAsync(clientDesktop);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        await ErrorReporter.MakeReport("UpdateClientDesktop(ClientDesktop clientDesktop)", ex);
        //        _logger.LogError(ex.Message);
        //        return BadRequest();
        //    }
        //}

        [HttpDelete]
        public async Task<ActionResult> DeleteClientDesktop(ClientDesktop clientDesktop)
        {
            try
            {
                await _clientDesktopsRepository.RemoveClientDesktopAsync(clientDesktop);
                return Ok();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("DeleteClientDesktop(ClientDesktop clientDesktop)", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
