﻿namespace CrtAdminPanel.Models.Interfaces
{
    public interface ISettings
    {
        bool PersonalKeyStore { get; set; }
        uint WarnDaysCount { get; set; }
        string DbFileName { get; set; }
        string BaseDirectory { get; }
        string DbPath { get; }
    }
}
