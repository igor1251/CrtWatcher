using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
//using Microsoft.Office.Interop.Excel;
using WA4D0G.Model.Interfaces;
using WA4D0G.Model.Classes;

namespace WA4D0G.MaintenanceTools
{
    public static class ExcelGenerator
    {
        public static async Task<bool> GenerateReportAsync(List<Certificate> unavailableCertificates)
        {
            Task generateReportTask = new Task(() =>
            {
                /*
                Application application = new Application();

                application.Visible = false;
                application.DisplayAlerts = false;

                application.SheetsInNewWorkbook = 1;
                Workbook workbook = application.Workbooks.Add(Type.Missing);
                Worksheet worksheet = (Worksheet)application.Worksheets[1];
                worksheet.Name = "Report";
                worksheet.Cells[1, 1].Value = "Subject";
                worksheet.Cells[1, 2].Value = "Phone";
                for (int i = 0; i < unavailableCertificates.Count; i++)
                {
                    worksheet.Cells[2 + i, 1].Value = unavailableCertificates[i].HolderFIO;
                    worksheet.Cells[2 + i, 2].Value = unavailableCertificates[i].HolderPhone;
                }

                string reportFileName = Environment.CurrentDirectory + "\\Reports\\report_" + TimeStampGenerator.GetDateTimeString(DateTime.Now) + ".xlsx";
                if (File.Exists(reportFileName))
                {
                    File.Delete(reportFileName);
                }

                application.Application.ActiveWorkbook.SaveAs(reportFileName);
                application.Quit();
                */
            });
            
            try
            {
                generateReportTask.Start();
                await generateReportTask;
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync("Error: Can't create report. " + ex.Message);
                return false;
            }

            await Logger.WriteAsync("Report generated.");
            return true;
        }
    }
}
