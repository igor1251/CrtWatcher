using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetworkOperators.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools.Reporters;
using X509KeysVault.Entities;
using X509KeysVault.Repositories;
using X509ObserverApi.Attributes;

namespace X509ObserverApi.Controllers
{
    [Authorization(Role.ADMINISTRATOR)]
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private ILogger<SubjectsController> _logger;
        private ISubjectsRepository _subjectsRepository;

        public SubjectsController(ILogger<SubjectsController> logger,
                                  ISubjectsRepository subjectsRepository)
        {
            _logger = logger;
            _subjectsRepository = subjectsRepository;
        }

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsAsync()
        {
            try
            {
                var subjects = await _subjectsRepository.GetSubjectsAsync();
                if (subjects == null)
                {
                    return NotFound();
                }
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetSubjectsAsync()", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Subject>> GetSubjectByID(int id)
        {
            try
            {
                var subject = await _subjectsRepository.GetSubjectByIDAsync(id);
                if (subject == null)
                {
                    return NotFound();
                }
                return Ok(subject);
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("GetSubjectByID(int id)", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddSubject(Subject subject)
        {
            try
            {
                await _subjectsRepository.AddSubjectAsync(subject);
                return Ok();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("AddSubject(Subject subject)", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSubject(Subject subject)
        {
            try
            {
                await _subjectsRepository.UpdateSubjectAsync(subject);
                return Ok();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("UpdateSubject(Subject subject)", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveSubject(int id)
        {
            try
            {
                var user = await _subjectsRepository.GetSubjectByIDAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                await _subjectsRepository.RemoveSubjectByIDAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                await ErrorReporter.MakeReport("RemoveSubject(int id)", ex);
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
