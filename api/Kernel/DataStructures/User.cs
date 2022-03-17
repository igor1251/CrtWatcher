using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DataStructures
{
    public class User : IUser
    {
        int _id;
        string _userName = string.Empty, _userPhone = string.Empty, _userComment = string.Empty;

        const string PHONE_TEMPLATE_REGULAR_EXPRESSION = @"^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$";

        List<Certificate> _certificates;

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
        [JsonPropertyName("userName")]
        public string UserName 
        { 
            get => _userName; 
            set
            {
                _userName = value;
            }
        }

        //[RegularExpression(PHONE_TEMPLATE_REGULAR_EXPRESSION, ErrorMessage = "Not a valid phone number")]
        [JsonPropertyName("userPhone")]
        public string UserPhone 
        { 
            get => _userPhone; 
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    _userPhone = "";
                }
                else if (Regex.IsMatch(value, PHONE_TEMPLATE_REGULAR_EXPRESSION)) _userPhone = value;
                else throw new ArgumentException("Not a valid phone number");
            }
        }

        [JsonPropertyName("userComment")]
        public string UserComment 
        { 
            get => _userComment;
            set => _userComment = value;
        }

        [Required]
        [JsonPropertyName("certificateList")]
        public List<Certificate> CertificateList 
        { 
            get
            {
                if (_certificates == null)
                {
                    _certificates = new List<Certificate>();
                }
                return _certificates;
            }
            set => _certificates = value;
        }
    }
}
