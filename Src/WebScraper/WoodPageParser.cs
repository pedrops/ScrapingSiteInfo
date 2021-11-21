using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebScraper
{
    public class WoodPageParser : WebPage
    {
        private void ParseString(string txt)
        {
            string companyName = string.Empty;
            string companyDescription = string.Empty;
            string companyAddress = string.Empty;
            string companyPhone = string.Empty;
            string companyFax = string.Empty;
            string companyEmail = string.Empty;
            string companyWebsite = string.Empty;
            string companyContactPerson = string.Empty;

            companyName = CleanText(Extract(txt, "<p class=\"heading_red\">", "<")).Trim();

            string validSection = Extract(txt.Replace("<img src=\"/woodworkers/images/2pix_green.gif\" width=\"100%\" height=\"1\">",
                                                                "<img src=\"/woodworkers/listing_images/"),
                                                                "<img src=\"/woodworkers/listing_images/", "WOODWEB, Inc.");
            companyDescription = Extract(validSection, "<br>", "<p>");
            companyDescription = CleanText(companyDescription.Contains("hspace=\"10\"") ? Extract(companyDescription + "||", "hspace=\"10\"", "||") : companyDescription);
            companyAddress = CleanText(Extract(validSection, "<p>", "Phone:")).Replace("\n", " ");
            companyPhone = CleanText(Extract(validSection, "Phone:", "<"));
            companyFax = CleanText(Extract(validSection, "Fax:", "\n"));
            companyFax = companyFax == "/" ? string.Empty : companyFax;
            companyEmail = CleanText(Extract(validSection, "E-mail: <a href=\"", "\""));
            companyEmail = string.IsNullOrEmpty(companyEmail) ? string.Empty : "http://www.woodindustry.com" + companyEmail;
            companyWebsite = CleanText(Extract(validSection, "Website: <a href=\"", "\""));
            companyWebsite = companyWebsite == "http://" ? string.Empty : companyWebsite;
            companyContactPerson = CleanText(Extract(validSection, "Contact Person:", "<"));
        }


        private string CleanText(string input)
        {
            string returnVal = input;
            while (returnVal.Contains("  "))
            {
                returnVal = returnVal.Replace("  ", " ");
            }
            return returnVal.Replace("&amp;", "&").Replace("'", "''").Replace("<br>", "").Replace(">", "").Replace("<p>", "").Replace("&nbsp;", " ").Replace("\r", " ").Replace("\n", " ").Trim();


        }


        private string Extract(string input, string start, string end)
        {
            string value = string.Empty;
            try
            {
                int s = input.IndexOf(start);
                int e = input.IndexOf(end, s + start.Length);
                if (s == -1 || e == -1)
                {
                    ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException();
                    throw ex;
                }

                s += start.Length;

                value = input.Substring(s, e - s);
            }
            catch (ArgumentOutOfRangeException)
            {
                value = "";
            }
            return value;
        }

    }
}
