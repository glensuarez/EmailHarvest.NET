using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OfficeOpenXml;

namespace ScrappingColegios
{
    public class EnviarUrlsAExcel
    {
        public void EnvioUrlsAExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string url = "http://www.centropolitecnico.cl/inicio/";
            string excelFilePath = "urls12.xlsx";

            List<string> urls = GetUrlsFromWebsite(url);

            SaveUrlsToExcel(urls, excelFilePath);

            Console.WriteLine("URLs saved to Excel successfully.");
        }

        private static List<string> GetUrlsFromWebsite(string url)
        {
            List<string> urls = new List<string>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            var linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
            if (linkNodes != null)
            {
                foreach (HtmlNode link in linkNodes)
                {
                    string href = link.GetAttributeValue("href", string.Empty);
                    if (!string.IsNullOrEmpty(href))
                    {
                        urls.Add(href);
                    }
                }
            }

            return urls;
        }

        private static void SaveUrlsToExcel(List<string> urls, string excelFilePath)
        {
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet;
                if (package.Workbook.Worksheets.Count == 0)
                {
                    worksheet = package.Workbook.Worksheets.Add("URLs");
                }
                else
                {
                    worksheet = package.Workbook.Worksheets[0];
                }

                int startRow = worksheet.Dimension?.Rows ?? 1;

                // Escribir las URLs en la hoja de Excel
                for (int i = 0; i < urls.Count; i++)
                {
                    worksheet.Cells[startRow + i, 1].Value = urls[i];
                }

                // Guardar el archivo de Excel
                package.Save();
            }
        }
    }
}

