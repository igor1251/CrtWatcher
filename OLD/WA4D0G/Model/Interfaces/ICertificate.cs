﻿using System;

namespace WA4D0G.Model.Interfaces
{
    public interface ICertificate
    {
        uint ID { get; set; }

        string HolderFIO { get; set; }

        string HolderPhone { get; set; }

        DateTime CertStartDateTime { get; set; }

        DateTime CertEndDateTime { get; set; }
    }
}