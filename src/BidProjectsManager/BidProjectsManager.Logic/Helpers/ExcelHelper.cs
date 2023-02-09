using BidProjectsManager.Model.Dto;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Enums;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;

namespace BidProjectsManager.Logic.Helpers
{
    public static class ExcelHelper
    {
        public static byte[] GetProjectExportData(this List<ProjectExportDto> projectsToExport)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Projects");
            package.Workbook.Properties.Created = DateTime.Now;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            ws.Cells[1, 1].LoadFromCollection(projectsToExport, true);

            var header = ws.Cells[1, 1, 1, ws.Dimension.End.Column];
            header.Style.Font.Bold = true;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));

            var typeProperties = typeof(ProjectExportDto).GetProperties();

            for (int i = 0; i < typeProperties.Length; i++)
            {
                var prop = typeProperties[i];
                var range = ws.Cells[2, i + 1, ws.Dimension.End.Row, i + 1];

                if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    range.Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                }

                if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    range.Style.Numberformat.Format = "0.00";
                }
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }

        public static byte[] GetGeneratorExportData(this List<string[]> dataToExport, List<string> columns)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Projects");
            package.Workbook.Properties.Created = DateTime.Now;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            for(int i = 0; i < columns.Count; i++)
            {
                ws.Cells[1, i+1].Value = columns[i];
            }
            for(int i = 0; i < dataToExport.Count; i++)
            {
                for(int j = 0; j < dataToExport[i].Length; j++)
                {
                    ws.Cells[i+2, j+1].Value = dataToExport[i][j];
                }
            }

            var header = ws.Cells[1, 1, 1, ws.Dimension.End.Column];
            header.Style.Font.Bold = true;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));

            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }

        public static byte[] GenerateProjectExportData(this Project project)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Projects");
            package.Workbook.Properties.Created = DateTime.Now;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            ws.Cells[1, 1].Value = "No"; ws.Cells[1, 2].Value = $"{project.Country.Code}{project.Id:D5}";
            ws.Cells[2, 1].Value = "Name"; ws.Cells[2, 2].Value = project.Name;
            ws.Cells[3, 1].Value = "Country"; ws.Cells[3, 2].Value = project.Country.Name;
            ws.Cells[4, 1].Value = "Stage"; ws.Cells[4, 2].Value = GetStageName(project.Stage);
            ws.Cells[5, 1].Value = "Status"; ws.Cells[5, 2].Value = GetStatusName(project.Status);
            ws.Cells[6, 1].Value = "Created By"; ws.Cells[6, 2].Value = project.CreatedBy;
            ws.Cells[7, 1].Value = "Created"; ws.Cells[7, 2].Value = project.Created;
            ws.Cells[7, 2].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            ws.Cells[8, 1].Value = "Modified By"; ws.Cells[8, 2].Value = project.ModifiedBy;
            ws.Cells[9, 1].Value = "Modified"; ws.Cells[9, 2].Value = project.Modified;
            ws.Cells[9, 2].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            ws.Cells[10, 1].Value = "Type"; ws.Cells[10, 2].Value = GetProjectTypeName(project.Type);
            ws.Cells[11, 1].Value = "Priority"; ws.Cells[11, 2].Value = GetPriorityName(project.Priority);
            ws.Cells[12, 1].Value = "Probability"; ws.Cells[12, 2].Value = GetProbabilityName(project.Probability);
            ws.Cells[13, 1].Value = "Number Of Vechicles"; ws.Cells[13, 2].Value = project.NumberOfVechicles;
            ws.Cells[14, 1].Value = "Lifetime In Thousands Kilometers"; ws.Cells[14, 2].Value = project.LifetimeInThousandsKilometers;
            ws.Cells[15, 1].Value = "Optional Extension In Years"; ws.Cells[15, 2].Value = project.OptionalExtensionYears;
            ws.Cells[16, 1].Value = "Description"; ws.Cells[16, 2].Value = project.Description;
            ws.Cells[17, 1].Value = "Start Of Bid Operation"; ws.Cells[17, 2].Value = project.BidOperationStart;
            ws.Cells[17, 2].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            ws.Cells[18, 1].Value = "Estimated End Of Bid Operation"; ws.Cells[18, 2].Value = project.BidEstimatedOperationEnd;
            ws.Cells[18, 2].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            ws.Cells[19, 1].Value = "Approval Date"; ws.Cells[19, 2].Value = project.ApprovalDate;
            ws.Cells[19, 2].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            ws.Cells[20, 1].Value = "Currency"; ws.Cells[20, 2].Value = project.ProjectCurrency.Code;
            ws.Cells[21, 1].Value = "Total Capex"; ws.Cells[21, 2].Value = project.TotalCapex;
            ws.Cells[21, 2].Style.Numberformat.Format = "0.00";
            ws.Cells[22, 1].Value = "Total Opex"; ws.Cells[22, 2].Value = project.TotalOpex;
            ws.Cells[22, 2].Style.Numberformat.Format = "0.00";
            ws.Cells[23, 1].Value = "Total Ebit"; ws.Cells[23, 2].Value = project.TotalEbit;
            ws.Cells[23, 2].Style.Numberformat.Format = "0.00";

            ws.Cells[1, 1, 23, 1].Style.Font.Bold = true;

            var counter = 24;
            if(project.Status == BidStatus.NoBid)
            {
                ws.Cells[counter, 1].Value = "Reason For No Bid"; ws.Cells[counter, 2].Value = project.NoBidReason;
                ws.Cells[counter++, 1].Style.Font.Bold = true;
            }

            if(project.Capexes.Count > 0)
            {
                counter += 2;
                ws.Cells[counter, 1].Value = "Capexes";
                ws.Cells[counter, 1].Style.Font.Bold = true;
                counter++;
                ws.Cells[counter, 1].Value = "Year"; ws.Cells[counter, 2].Value = "Value";
                ws.Cells[counter, 1,counter,2].Style.Font.Bold = true;
                foreach (var item in project.Capexes)
                {
                    counter++;
                    ws.Cells[counter, 1].Value = item.Year; ws.Cells[counter, 2].Value = item.Value;
                    ws.Cells[counter, 2].Style.Numberformat.Format = "0.00";
                    ws.Cells[counter, 1].Style.Font.Bold = true;
                }
            }

            if (project.Opexes.Count > 0)
            {
                counter += 2;
                ws.Cells[counter, 1].Value = "Opexes";
                ws.Cells[counter, 1].Style.Font.Bold = true;
                counter++;
                ws.Cells[counter, 1].Value = "Year"; ws.Cells[counter, 2].Value = "Value";
                ws.Cells[counter, 1].Style.Font.Bold = true;
                ws.Cells[counter, 1].Value = "Year"; ws.Cells[counter, 2].Value = "Value";
                ws.Cells[counter, 1, counter, 2].Style.Font.Bold = true;

                foreach (var item in project.Opexes)
                {
                    counter++;
                    ws.Cells[counter, 1].Value = item.Year; ws.Cells[counter, 2].Value = item.Value;
                    ws.Cells[counter, 1].Style.Font.Bold = true;
                    ws.Cells[counter, 2].Style.Numberformat.Format = "0.00";
                }
            }

            if (project.Ebits.Count > 0)
            {
                counter += 2;
                ws.Cells[counter, 1].Value = "Ebits";
                ws.Cells[counter, 1].Style.Font.Bold = true;
                counter++;
                ws.Cells[counter, 1].Value = "Year"; ws.Cells[counter, 2].Value = "Value";
                ws.Cells[counter, 1].Style.Font.Bold = true;
                ws.Cells[counter, 1].Value = "Year"; ws.Cells[counter, 2].Value = "Value";
                ws.Cells[counter, 1, counter, 2].Style.Font.Bold = true;

                foreach (var item in project.Ebits)
                {
                    counter++;
                    ws.Cells[counter, 1].Value = item.Year; ws.Cells[counter, 2].Value = item.Value;
                    ws.Cells[counter, 1].Style.Font.Bold = true;
                    ws.Cells[counter, 2].Style.Numberformat.Format = "0.00";
                }
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }

        private static string GetStageName(ProjectStage stage)
        {
            return stage switch
            {
                ProjectStage.Approved => "Approved",
                ProjectStage.Rejected => "Rejected",
                ProjectStage.Draft => "Draft",
                ProjectStage.Submited => "Submitted",
                _ => "Unknown"
            };
        }

        private static string GetStatusName(BidStatus? status)
        {
            return status switch
            {
                BidStatus.Won => "Won",
                BidStatus.NoBid => "No Bid",
                BidStatus.AwaitingSignature => "Awaiting Signature",
                BidStatus.Lost => "Lost",
                BidStatus.BidPreparation => "Bid Preparation",
                _ => "Unknown"
            };
        }

        private static string GetProjectTypeName(ProjectType? type)
        {
            return type switch
            {
                ProjectType.Acquisition => "Acquisition",
                ProjectType.TenderOffer => "Tender Offer",
                _ => "Unknown"
            };
        }

        private static string GetProbabilityName(BidProbability? probability)
        {
            return probability switch
            {
                BidProbability.High => "High",
                BidProbability.Medium => "Medium",
                BidProbability.Low => "Low",
                _ => "Unknown"
            };
        }

        private static string GetPriorityName(BidPriority? priority)
        {
            return priority switch
            {
                BidPriority.High => "High",
                BidPriority.Medium => "Medium",
                BidPriority.Low => "Low",
                _ => "Unknown"
            };
        }
    }
}
