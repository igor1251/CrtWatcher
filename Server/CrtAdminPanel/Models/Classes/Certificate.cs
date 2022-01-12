using System;
using System.ComponentModel;
using CrtAdminPanel.Models.Interfaces;

namespace CrtAdminPanel.Models.Classes
{
    public class Certificate : ICertificate, INotifyPropertyChanged
    {
        private uint _id;

        private string _holderFio, _holderPhone;

        private DateTime _certStartDateTime, _certEndDateTime;

        public uint ID
        {
            get => _id;

            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string HolderFIO
        {
            get => string.IsNullOrEmpty(_holderFio) ? "---" : _holderFio;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Invalid parameter was send for initialization _holderFio field");
                }

                _holderFio = value;
                OnPropertyChanged("HolderFIO");
            }
        }

        public string HolderPhone
        {
            get => string.IsNullOrEmpty(_holderPhone) ? "---" : _holderPhone;

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Invalid parameter was send for initialization _holderPhone field");
                }

                _holderPhone = value;
                OnPropertyChanged("HolderPhone");
            }
        }

        public DateTime CertStartDateTime
        {
            get => _certStartDateTime;

            set
            {
                _certStartDateTime = value;
                OnPropertyChanged("CertStartDateTime");
            }
        }

        public DateTime CertEndDateTime
        {
            get => _certEndDateTime;

            set
            {
                _certEndDateTime = value;
                OnPropertyChanged("CertEndDateTime");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
