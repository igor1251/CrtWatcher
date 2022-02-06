using ElectronicDigitalSignatire.Models.Interfaces;
using ElectronicDigitalSignature.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElectronicDigitalSignature.Models.Classes
{
    internal class Workstation : IWorkstation
    {
        private string _ip;
        private IEnumerable<ICertificateSubject> _subjectList;

        [Required]
        [JsonPropertyName("ip")]
        public string IP 
        { 
            get => _ip; 
            set => _ip = value; 
        }

        public IEnumerable<ICertificateSubject> SubjectsList 
        { 
            get
            {
                if (_subjectList == null)
                {
                    _subjectList = new List<ICertificateSubject>();
                }
                return _subjectList;
            }
            set => _subjectList = value; 
        }
    }
}
