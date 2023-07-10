using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ScrappingColegios
{
    public class MasDeUnaUrl
    {

        public void ScrappingMasDeUnaUrl()
        {

            List<string> urls = new List<string>()
            {
                "http://heinrich.cl/new/",
                "http://heinrich.cl/ggh2/",
                "http://heinrich.cl/ggh1/",
                "http://www.heinrich.cl/big/"
            };

            List<string> emailAddresses = new List<string>();

            foreach (string url in urls)
            {
                string html = GetHtmlFromUrl(url);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                foreach (var link in doc.DocumentNode.Descendants("a"))
                {
                    string href = link.GetAttributeValue("href", "");
                    if (href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                    {
                        string emailAddress = href.Substring(7);
                        emailAddresses.Add(emailAddress);
                    }
                }
            }

            Console.WriteLine("Email addresses found:");
            foreach (string emailAddress in emailAddresses)
            {
                Console.WriteLine(emailAddress);
            }
        }

        private static string GetHtmlFromUrl(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
