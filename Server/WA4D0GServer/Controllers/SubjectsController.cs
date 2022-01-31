using ElectrnicDigitalSignatire.Models.Classes;
using ElectrnicDigitalSignatire.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WA4D0GServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectsController : ControllerBase
    {
        private IDbStore _dbStore;

        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(ILogger<SubjectsController> logger,
                                  IDbStore dbStore)
        {
            _logger = logger;
            _dbStore = dbStore;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificateSubject>>> GetSubjectsAsync()
        {
            _logger.LogInformation("Loading subjects list");
            var subjectsList = await _dbStore.GetSubjects();

            if (subjectsList == null)
            {
                return NotFound();
            }

            return subjectsList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CertificateSubject>> GetSubjectsAsync(int id)
        {
            _logger.LogInformation("Loading subject by id");
            var subject = await _dbStore.GetSubjectByID(id);

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSubjectAsync(CertificateSubject subject)
        {
            _logger.LogInformation("Creating new subject. Subject info:\n" + subject.SubjectName + "\n" + subject.SubjectPhone + "\n" + subject.SubjectComment);
            await _dbStore.InsertSubject(subject);
            _logger.LogInformation("Done");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubjectsAsync(int id, CertificateSubject subject)
        {
            if (id != subject.ID)
            {
                return BadRequest();
            }

            await _dbStore.UpdateSubject(subject);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubjectAsync(int id)
        {
            _logger.LogInformation("Deliting subject under id=" + id.ToString());
            var subject = _dbStore.GetSubjectByID(id);
            if (subject == null)
            {
                _logger.LogInformation("Requested subject not found");
                return NotFound();
            }
            await _dbStore.DeleteSubject(id);
            _logger.LogInformation("Successfully deleted");
            return NoContent();
        }
    }
}
