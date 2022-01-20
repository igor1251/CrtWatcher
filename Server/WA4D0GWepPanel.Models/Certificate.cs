using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WA4D0GWebPanel.Models
{
    public class Certificate
    {
        [Required]
        public int ID { get; set; }
        
        [Required]
        public Subject Subject { get; set; }
        
        [Required]
        public string Algorithm { get; set; }
        
        [Required]
        public string Hash { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
