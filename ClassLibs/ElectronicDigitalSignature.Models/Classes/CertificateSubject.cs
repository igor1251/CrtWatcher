﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ElectronicDigitalSignatire.Models.Interfaces;

namespace ElectronicDigitalSignatire.Models.Classes
{
    public class CertificateSubject : ICertificateSubject
    {
        int _id;
        string _subjectName, _subjectPhone = string.Empty, _subjectComment = string.Empty;

        const string PHONE_TEMPLATE_REGULAR_EXPRESSION = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";

        List<CertificateData> _certificates;

        [Required]
        [JsonPropertyName("id")]
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
        [JsonPropertyName("subjectName")]
        public string SubjectName 
        { 
            get => _subjectName; 
            set
            {
                _subjectName = value;
            }
        }

        [RegularExpression(PHONE_TEMPLATE_REGULAR_EXPRESSION, ErrorMessage = "Not a valid phone number")]
        [JsonPropertyName("subjectPhone")]
        public string SubjectPhone 
        { 
            get => _subjectPhone; 
            set
            {
                _subjectPhone = value;
            }
        }

        [JsonPropertyName("subjectComment")]
        public string SubjectComment 
        { 
            get => _subjectComment;
            set => _subjectComment = value;
        }

        [Required]
        [JsonPropertyName("certificateList")]
        public List<CertificateData> CertificateList 
        { 
            get
            {
                if (_certificates == null)
                {
                    _certificates = new List<CertificateData>();
                }
                return _certificates;
            }
            set => _certificates = value;
        }
    }
}
