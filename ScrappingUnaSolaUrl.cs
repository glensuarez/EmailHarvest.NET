using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ScrappingColegios
{
    public class ScrappingUnaSolaUrl
    {


        public void Metodounasolaurl()
        {
         HtmlWeb ohtml = new HtmlWeb();
        HtmlDocument doc = ohtml.Load("http://heinrich.cl/ggh2/");

        List<string> emailAddresses = new List<string>();

        foreach (var link in doc.DocumentNode.Descendants("a"))
        {
            string href = link.GetAttributeValue("href", "");
            if (href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
            {
                string emailAddress = href.Substring(7);
        emailAddresses.Add(emailAddress);
            }
         }

        Console.WriteLine("Email addresses found:");
        foreach (string emailAddress in emailAddresses)
        {
            Console.WriteLine(emailAddress);
        }

        }
       
    }
}
