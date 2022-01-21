﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WA4D0GWebPanel.Models;

namespace WA4D0GWebPanel.Services
{
    public interface ICertificateRepository
    {
        Task<IEnumerable<Subject>> GetSubjectsList();
        Task DeleteSubject(int subjectID);
        Task EditSubject(Subject subject);
    }
}
