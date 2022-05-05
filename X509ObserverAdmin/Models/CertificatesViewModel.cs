﻿using System.Collections.Generic;
using X509KeysVault.Entities;

namespace X509ObserverAdmin.Models
{
    public class CertificatesViewModel
    {
        public List<Subject> Subjects { get; set; } = new List<Subject>()
        {
            new Subject() { ID = 1, Name = "test", Phone = "89206005603" }
        };
    }
}
