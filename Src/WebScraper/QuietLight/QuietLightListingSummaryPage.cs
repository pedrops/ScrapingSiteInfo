using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using HtmlAgilityPack;

namespace WebScraper
{
    public class QuietLightListingSummaryPage : WebPage
    {

        public List<QuietLightListingSummary> Summaries { get; set; } = new List<QuietLightListingSummary>();
        private string Url { get; set; }
        private string FileName { get; set; }

        public QuietLightListingSummaryPage(bool loadIfNotFound = false)
        {
            Url = $"https://quietlight.com/listings/";
            FileName = $"D:\\Solutions\\WoodScraper\\DownloadedPages\\QuietLightSummary.html";

            if (File.Exists(FileName)) LoadFromFile(FileName);
            else if (loadIfNotFound)
            {
                LoadFromWeb(Url, "").Wait();
                SaveToFile(FileName);
            }
        }

        public void DownloadAllListings(int numListings)
        {
            var counter = 0;
            foreach (var summary in Summaries.Where(s => s.Id > 0))
            {
                var listing = new QuietLightListingPage(summary.Id, true);
                if (!listing.LoadedFromFile)
                {
                    Thread.Sleep(60 * 1000);
                    counter++;
                }

                if (counter > numListings) break;
            }
        }

        public override void LoadFromFile(string fullFilePath)
        {
            base.LoadFromFile(fullFilePath);

            var allContentNodes = Doc.DocumentNode.SelectNodes("//div[contains(@class, 'single-content')]");
            foreach (var allContentNode in allContentNodes)
            {
                ProcessOneContentNode(allContentNode);
            }
        }

        private void ProcessOneContentNode(HtmlNode node)
        {
            var newListing = new QuietLightListingSummary();
            var classNames = node.GetClasses().ToList();
            if (classNames.Contains("sold")) newListing.Status = BusinessStatus.Sold;
            else if (classNames.Contains("under-loi")) newListing.Status = BusinessStatus.LetterOfIntent;
            else if (classNames.Contains("public-listing")) newListing.Status = BusinessStatus.ForSale;

            newListing.Id = GetId(node);
            if (newListing.Status != BusinessStatus.Unknown) Summaries.Add(newListing);
        }

        private int GetId(HtmlNode listingNode)
        {
            var hrefParent = listingNode.SelectNodes("div[contains(@class, 'recent_1')]").First();
            var hrefNode = hrefParent.SelectSingleNode("a");
            if (hrefNode == null) return 0;
            var urlAttr = hrefNode.Attributes.First(a => a.Name == "href");
            var numStr = urlAttr.Value.Replace("https://quietlight.com/listings/", "").Replace("/","");
            if (int.TryParse(numStr, out int res)) return res;
            return 0;
        }
    }
}
