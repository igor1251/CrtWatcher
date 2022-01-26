using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ElectrnicDigitalSignatire.Models.Interfaces;

namespace ElectrnicDigitalSignatire.Models.Classes
{
    public class CertificateSubject : ICertificateSubject
    {
        int _id;
        string _subjectName, _subjectPhone = "---", _subjectComment = "---";
        List<CertificateData> _certificates = new List<CertificateData>();

        [Required]
        public int ID 
        { 
            get => _id; 
            set
            {
                if (value < 0) throw new ArgumentException("ID must be above or equal to '0'");
                else _id = value;
            }
        }

        [Required]
        public string SubjectName 
        { 
            get => _subjectName; 
            set
            {
                _subjectName = value;
            }
        }

        [Phone(ErrorMessage = "Not a valid phone number")]
        public string SubjectPhone 
        { 
            get => _subjectPhone; 
            set
            {
                _subjectPhone = value;
            }
        }

        public string SubjectComment 
        { 
            get => _subjectComment;
            set => _subjectComment = value;
        }

        [Required]
        public List<CertificateData> CertificateList 
        { 
            get => _certificates; 
            set => _certificates = value;
        }
    }
}
