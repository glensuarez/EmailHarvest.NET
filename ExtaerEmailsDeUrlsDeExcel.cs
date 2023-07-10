using HtmlAgilityPack;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ScrappingColegios
{
    public class ExtaerEmailsDeUrlsDeExcel
    {
        public void ExtrayendoEmailsDeUrlsDeExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string excelFilePath = "urls12.xlsx";
            string newExcelFilePath = "basedatos11junio.xlsx";

            List<string> emailAddresses = ExtractEmailAddressesFromExcel(excelFilePath);

            Console.WriteLine("Email addresses found:");
            foreach (string emailAddress in emailAddresses)
            {
                Console.WriteLine(emailAddress);
            }

            SaveEmailAddressesToExcel(emailAddresses, newExcelFilePath);
        }

        private static List<string> ExtractEmailAddressesFromExcel(string excelFilePath)
        {
            List<string> emailAddresses = new List<string>();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        string url = worksheet.Cells[row, 1].Value?.ToString();
                        if (!string.IsNullOrEmpty(url))
                        {
                            List<string> urlsEmails = GetEmailAddressesFromWebsite(url);
                            emailAddresses.AddRange(urlsEmails);
                        }
                    }
                }
            }

            return emailAddresses;
        }

        private static List<string> GetEmailAddressesFromWebsite(string url)
        {
            List<string> emailAddresses = new List<string>();

            string html = GetHtmlFromUrl(url);

            // Expresión regular para buscar direcciones de correo electrónico
            string emailPattern = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}";

            // Buscar coincidencias en el HTML
            MatchCollection matches = Regex.Matches(html, emailPattern);

            // Agregar direcciones de correo electrónico encontradas a la lista
            foreach (Match match in matches)
            {
                string emailAddress = match.Value;
                emailAddresses.Add(emailAddress);
            }

            return emailAddresses;
        }

        private static string GetHtmlFromUrl(string url)
        {
            string html = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                html = reader.ReadToEnd();
                            }
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.Redirect)
                    {
                        string newUrl = response.Headers["Location"];
                        if (!string.IsNullOrEmpty(newUrl))
                        {
                            html = GetHtmlFromUrl(newUrl);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Manejar la excepción si ocurre algún error en la solicitud web
                // ...
            }

            return html;
        }



        private static void SaveEmailAddressesToExcel(List<string> emailAddresses, string excelFilePath)
        {
            FileInfo file = new FileInfo(excelFilePath);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                {
                    worksheet = package.Workbook.Worksheets.Add("Emails");
                }

                int rowCount = worksheet.Dimension?.Rows ?? 0;

                for (int i = 0; i < emailAddresses.Count; i++)
                {
                    worksheet.Cells[rowCount + 1 + i, 1].Value = emailAddresses[i];
                }

                package.Save();
            }
        }
    }
}

