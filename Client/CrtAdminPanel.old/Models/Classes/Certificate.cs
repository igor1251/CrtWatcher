using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CrtAdminPanel.Models.Interfaces;

namespace CrtAdminPanel.Models.Classes
{
    public class Certificate : ICertificate
    {
        private uint _id;

        private string _holderFio, _holderPhone;

        private DateTime _certStartDateTime, _certEndDateTime;

        [ReadOnly(true)]
        public uint ID
        {
            get => _id;

            set
            {
                _id = value;
            }
        }

        [ReadOnly(true)]
        public string HolderFIO
        {
            get => string.IsNullOrEmpty(_holderFio) ? "---" : _holderFio;

            set
            {
                _holderFio = value;
            }
        }
        
        [Required]
        [Phone]
        public string HolderPhone
        {
            get => string.IsNullOrEmpty(_holderPhone) ? "---" : _holderPhone;

            set
            {
                _holderPhone = value;
            }
        }

        [ReadOnly(true)]
        public DateTime CertStartDateTime
        {
            get => _certStartDateTime;

            set
            {
                _certStartDateTime = value;
            }
        }

        [ReadOnly(true)]
        public DateTime CertEndDateTime
        {
            get => _certEndDateTime;

            set
            {
                _certEndDateTime = value;
            }
        }
    }
}
