using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PttLib.Helpers
{
    public static class ExcelImportHelper
    {
        public static DataTable GetDataTableFromExcel(string path, IList<string> columnsToTake, bool hasHeader = true, int sheetNumber = 0)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets[sheetNumber + 1];
                if (ws.Dimension == null) return null;
                DataTable tbl = new DataTable();

                var colHeaderIndex = 0;
                foreach (var col in columnsToTake)
                {
                    var colHeader = ws.Cells[col + "1"].Text;
                    tbl.Columns.Add(hasHeader ? String.Format("{0} {1}", colHeader, colHeaderIndex) : String.Format("Column {0}", colHeaderIndex));
                    colHeaderIndex++;
                }

                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var row = tbl.NewRow();
                    var colIndex = 0;
                    foreach (var col in columnsToTake)
                    {
                        var cellValue = ws.Cells[col + rowNum].Text;
                        row[colIndex] = cellValue;
                        colIndex++;
                    }
                    tbl.Rows.Add(row);
                }
                return tbl;
            }
        }

        /// <summary>
        /// gelen otel adindan kategori id sini parse etmeye calis
        /// </summary>
        /// <param name="hotelName"></param>
        /// <returns></returns>
        public static int ParseCategoryId(string hotelName)
        {
            var parsedCategoryNumber = ParsedCategoryNumber(hotelName);
            if (parsedCategoryNumber == null)
            {
                var categoryName = ParsedCategoryName(hotelName);
                if(String.IsNullOrEmpty(categoryName)) return 0;

                if(!Dictionnaries.HotelCategories.ContainsValue(categoryName)) return 0;
                return Dictionnaries.HotelCategories.FirstOrDefault(x => x.Value == categoryName).Key;
                 
            }

            return (Int32.Parse(parsedCategoryNumber.Item1)*2-3 + (parsedCategoryNumber.Item2?1:0));
        }

        private static string ParsedCategoryName(string hotelName)
        {
            //sonra HVx
            var match = Regex.Match(hotelName, @"(HV\d)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            match = Regex.Match(hotelName, @"(SC)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }

        private static Tuple<string, bool> ParsedCategoryNumber(string hotelName)
        {
            //gelen: ADORA GOLF RESORT HOTEL (5*), BELEK
            
            //ilk once x*+
            var match = Regex.Match(hotelName, @"(\d)\s*\*\s*\+", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return new Tuple<string, bool>( match.Groups[1].Value, true);
            }
            //sonra x+*
            match = Regex.Match(hotelName, @"(\d)\s*\+\s*\*", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return new Tuple<string, bool>(match.Groups[1].Value, true);
            }
            //sonra x*
            match = Regex.Match(hotelName, @"(\d)\s*\*", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return new Tuple<string, bool>(match.Groups[1].Value, false);
            }
            
            
            return null;
        }
    }
}