using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PttLib.Tours;

namespace PttLib.Helpers
{
    public class ExcelReport
    {
        private const string REPORT_FILE_FORMAT2 =@"..\..\reports\FiyatAnaliz_{0}.xlsx";

        private static void CreateReportsFolder()
        {
            var path = Path.GetFullPath(Path.Combine(Helper.AssemblyDirectory ,@"..\..\reports"));
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);

        }
     
        public static void BuildExcelFile(List<Tour> tourList, string destination)
        {
            CreateReportsFolder();
            var excelFile = Path.GetFullPath(Path.Combine(Helper.AssemblyDirectory, string.Format(REPORT_FILE_FORMAT2, String.Format("{0:dd.MM.yyyy_HH.mm.ss}", DateTime.Now))));

            FileInfo newFile = new FileInfo(excelFile);

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("DATA");

                worksheet.Column(1).Width = 10;
                worksheet.Column(2).Width = 40;
                worksheet.Column(3).Width = 40;
                worksheet.Column(6).Width = 13;
                worksheet.Column(8).Width = 20;
                worksheet.Column(9).Width = 20;
                worksheet.Column(10).Width = 20;
                worksheet.Column(11).Width = 20;
                worksheet.Column(1).Style.Numberformat.Format = @"dd/mm/yyyy";
                worksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;


                //Add the headers
                worksheet.Cells[1, 1].Value = "DATE";
                worksheet.Cells[1, 2].Value = "HOTEL";
                worksheet.Cells[1, 3].Value = "ROOM TYPE";
                worksheet.Cells[1, 4].Value = "NIGHT";
                worksheet.Cells[1, 5].Value = "MEAL";
                worksheet.Cells[1, 6].Value = "ACC";
                worksheet.Cells[1, 7].Value = "PRICE";
                worksheet.Cells[1, 8 ].Value = "T.O.";
                worksheet.Cells[1, 9 ].Value = "CITY";
                worksheet.Cells[1, 10 ].Value = "DESTINATION";
                worksheet.Cells[1, 11 ].Value = "ISSUE DATE";

                worksheet.Cells["A1:K1"].Style.Font.Bold = true;
                worksheet.Cells["A1:K1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:K1"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells["A1:K1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                var issueDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                
                var fgColor = Color.Black;
                var bgColor = Color.LightBlue;

                var altfgColor = Color.Black;
                var altbgColor = Color.White;

                var curfgColor = fgColor;
                var curbgColor = bgColor;

                var lastHotelName = "";
                //Add some items...
                var cursor = 2;
                foreach (var item in tourList)
                {
                    if (lastHotelName != item.HotelCommonName)
                    {
                        if(curfgColor==fgColor && curbgColor==bgColor)
                        {
                            curfgColor = altfgColor;
                            curbgColor = altbgColor;
                        }
                        else
                        {
                            curfgColor = fgColor;
                            curbgColor = bgColor;                            
                        }
                        lastHotelName = item.HotelCommonName;                        
                    }

                    worksheet.Cells[cursor, 1].Value = item.Date.ToString("dd.MM.yyyy");
                    int nightCount = 0;
                    Int32.TryParse(item.Night, out nightCount);
                    worksheet.Cells[cursor, 2].Value = item.HotelCommonName;
                    worksheet.Cells[cursor, 3].Value = item.RoomType;

                    worksheet.Cells[cursor, 4].Value = nightCount.ToString();
                    worksheet.Cells[cursor, 5].Value = item.Meal;
                    worksheet.Cells[cursor, 6].Value = item.ACC;
                    worksheet.Cells[cursor, 7].Value = item.Price;
                    worksheet.Cells[cursor, 8 ].Value = item.TO;
                    worksheet.Cells[cursor, 9 ].Value = item.City;
                    worksheet.Cells[cursor, 10 ].Value = destination.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    worksheet.Cells[cursor, 11 ].Value = issueDate; //item.IssueDate.ToString("dd.MM.yyyy HH:mm");

                    worksheet.Cells["A"+cursor.ToString()+":K"+cursor.ToString()].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A" + cursor.ToString() + ":K" + cursor.ToString()].Style.Font.Color.SetColor(curfgColor);
                    worksheet.Cells["A" + cursor.ToString() + ":K" + cursor.ToString()].Style.Fill.BackgroundColor.SetColor(curbgColor);


                    cursor++;

                }
                // save our new workbook and we are done!
                package.Save();

            }

            Process.Start(excelFile);

        }

    }
}
