﻿using System;

namespace ElectrnicDigitalSignatire.Models.Interfaces
{
    public interface ICertificateData
    {
        int ID { get; set; }
        string CertificateHash { get; set; }
        string Algorithm { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}