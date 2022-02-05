using ElectrnicDigitalSignatire.Models.Classes;
using ElectrnicDigitalSignatire.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WA4D0GServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ILogger<SubjectsController> _logger;
        private IDbStore _dbStore;
        private ILocalStore _localStore;

        public SubjectsController(ILogger<SubjectsController> logger,
                                  IDbStore dbStore,
                                  ILocalStore localStore)
        {
            _logger = logger;
            _dbStore = dbStore;
            _localStore = localStore;
        }

        #region GET methods

        [HttpGet]
        [Route("db")]
        public async Task<ActionResult<IEnumerable<CertificateSubject>>> GetSubjectsFromDbAsync()
        {
            _logger.LogInformation("Loading subjects list from database");
            var subjectsList = await _dbStore.GetSubjects();

            if (subjectsList == null)
            {
                _logger.LogInformation("Subjects list is empty");
                return NotFound();
            }

            _logger.LogInformation("Loaded");
            return Ok(subjectsList);
        }

        [HttpPut]
        [Route("system")]
        public async Task<ActionResult> LoadSubjectsFromLocalStoreAsync()
        {
            _logger.LogInformation("Loading subjects list from local store");
            var subjectsList = await _localStore.LoadCertificateSubjectsAndCertificates();
            await _dbStore.InsertSubject(subjectsList);

            if (subjectsList == null)
            {
                _logger.LogInformation("Subjects list is empty");
                return NotFound();
            }

            _logger.LogInformation("Loaded");
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CertificateSubject>> GetSubjectsAsync(int id)
        {
            _logger.LogInformation("Loading subject by id=" + id);
            var subject = await _dbStore.GetSubjectByID(id);

            if (subject == null)
            {
                _logger.LogWarning("Subject not found");
                return NotFound();
            }

            _logger.LogInformation("Loaded");
            return Ok(subject);
        }

        #endregion

        #region POST methods

        [HttpPost]
        public async Task<ActionResult> CreateSubjectAsync(CertificateSubject subject)
        {
            _logger.LogInformation("Creating new subject. Subject info:\n" + subject.SubjectName + "\n" + subject.SubjectPhone + "\n" + subject.SubjectComment);
            await _dbStore.InsertSubject(subject);
            _logger.LogInformation("Done");
            //returns 201-code
            return new CreatedResult(new Uri("api/subjects"), new { message = "New subject successfully created" });
        }

        #endregion

        #region PUT methods

        [HttpPut]
        public async Task<ActionResult> UpdateSubjectsAsync(CertificateSubject subject)
        {
            _logger.LogInformation("Updating subject under id=" + subject.ID.ToString());

            await _dbStore.UpdateSubject(subject);
            _logger.LogInformation("Done");
            return Ok();
        }

        #endregion

        #region DELETE methods

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubjectAsync(int id)
        {
            _logger.LogInformation("Deliting subject under id=" + id.ToString());
            var subject = _dbStore.GetSubjectByID(id);
            if (subject == null)
            {
                _logger.LogWarning("Requested subject not found");
                return NotFound();
            }
            await _dbStore.DeleteSubject(id);
            _logger.LogInformation("Successfully deleted");
            return Ok();
        }

        #endregion
    }
}
