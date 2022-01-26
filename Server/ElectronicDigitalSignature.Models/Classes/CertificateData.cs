﻿using ElectrnicDigitalSignatire.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElectrnicDigitalSignatire.Models.Classes
{
    public class CertificateData : ICertificateData
    {
        int _id;
        string _certificateHash, _algorithm;
        DateTime _startDate, _endDate;

        [Required]
        public int ID 
        {
            get => _id;
            set
            {
                if (value < 0) throw new ArgumentException("ID must be above or equal '0'");
                else _id = value;
            }
        }

        [Required]
        public string CertificateHash 
        { 
            get => _certificateHash; 
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value)) 
                    throw new ArgumentException("Certificate hash cannot be an empty string, a space, or null");
                else _certificateHash = value;
            }
        }

        [Required]
        public string Algorithm 
        { 
            get => _algorithm; 
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Algorithm hash cannot be an empty string, a space, or null");
                else _algorithm = value;
            }
        }

        [Required]
        public DateTime StartDate 
        { 
            get => _startDate; 
            set => _startDate = value; 
        }

        [Required]
        public DateTime EndDate 
        { 
            get => _endDate; 
            set => _endDate = value; 
        }
    }
}
