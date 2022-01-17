﻿using System;

namespace CrtLoader.Model.Interfaces
{
    public interface ICertificateData
    {
        int ID { get; set; }
        ICertificateSubject Subject { get; set; }
        string CertificateHash { get; set; }
        string Algorithm { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}
