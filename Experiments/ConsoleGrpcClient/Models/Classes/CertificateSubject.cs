using System;
using System.Collections.Generic;
using CrtLoader.Model.Interfaces;

namespace CrtLoader.Model.Classes
{
    public class CertificateSubject : ICertificateSubject
    {
        int _id;
        string _subjectName, _subjectPhone = "---", _subjectComment = "---";
        List<CertificateData> _certificates = new List<CertificateData>();

        public int ID 
        { 
            get => _id; 
            set
            {
                if (value < 0) throw new ArgumentException("ID must be above or equal to '0'");
                else _id = value;
            }
        }
        public string SubjectName 
        { 
            get => _subjectName; 
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Subject name cannot be an empty string, a space, or null");
                else _subjectName = value;
            }
        }
        public string SubjectPhone 
        { 
            get => _subjectPhone; 
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Subject phone cannot be an empty string, a space, or null");
                else _subjectPhone = value;
            }
        }
        public string SubjectComment 
        { 
            get => _subjectComment;
            set => _subjectComment = value;
        }
        public List<CertificateData> CertificateList 
        { 
            get => _certificates; 
            set => _certificates = value;
        }
    }
}
