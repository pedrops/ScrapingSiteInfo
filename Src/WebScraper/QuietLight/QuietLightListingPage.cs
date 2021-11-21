using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebScraper
{
    public class QuietLightListingPage : WebPage
    {

        public QuietLightBizForSale Listing { get; set; }

        public QuietLightListingPage(int num, bool loadIfNotFound = false)
        {
            Listing = new QuietLightBizForSale();
            Listing.Status = BusinessStatus.ForSale;
            Listing.ListingNum = num;
            Listing.FileName = $"D:\\Solutions\\WoodScraper\\DownloadedPages\\QuietLight{num}.html";
            Listing.Url = $"https://quietlight.com/listings/{num}";
            if (File.Exists(Listing.FileName)) LoadFromFile(Listing.FileName);
            else if (loadIfNotFound)
            {
                LoadFromWeb(Listing.Url, "").Wait();
                SaveToFile(Listing.FileName);
            }
        }

        public void SaveToFile()
        {
            base.SaveToFile(Listing.FileName);
        }

        public async Task Refresh()
        {
            await LoadFromWeb(Listing.Url, "");
        }

        public override void LoadFromFile(string fullFilePath)
        {
            base.LoadFromFile(fullFilePath);
            Listing.Title = GetTitle(Doc);
            Listing.Revenue = GetRevenue(Doc);
            Listing.Income = GetIncome(Doc);
            Listing.Multiple = GetMultiple(Doc);
            Listing.AskingPrice = GetAskingPrice(Doc);
            Listing.BusinessHasInventory = GetBusinessHasInventory(Doc);
            Listing.PriceIncludesInventory = GetPriceIncludesInventory(Doc);
            Listing.Description = GetDescription(Doc);
        }

        private string GetDescription(HtmlDocument doc)
        {
            var revNode = GetAskingPriceNode(doc);
            if (revNode == null) return "";

            var parentDiv = revNode.ParentNode;
            var pNodes = parentDiv.ChildNodes;
            var txt = "";
            foreach (var pNode in pNodes.Where(p => p.Name == "p"))
            {
                txt += pNode.InnerText;
            }

            return txt;
        }

        private string GetTitle(HtmlDocument doc)
        {
            return Doc.DocumentNode.SelectNodes("//h3").First().InnerText;
        }

        private bool? GetBusinessHasInventory(HtmlDocument doc)
        {
            var revNode = GetAskingPriceNode(doc);
            if (revNode == null) return false;
            if (revNode.InnerText.Contains("+ Inventory")) return true;
            return null;
        }

        private bool? GetPriceIncludesInventory(HtmlDocument doc)
        {
            var revNode = GetAskingPriceNode(doc);
            if (revNode == null) return false;
            if (revNode.InnerText.Contains("+ Inventory")) return false;
            return null;
        }

        private HtmlNode GetAskingPriceNode(HtmlDocument doc)
        {
            var revNodes= doc.DocumentNode.SelectNodes("//h4");
            if (revNodes == null) return null;
            var revNode= revNodes.FirstOrDefault(n => n.InnerText.Contains("Asking Price"));
            return revNode;
        }

        private int GetAskingPrice(HtmlDocument doc)
        {
            var revNode = GetAskingPriceNode(doc);
            if (revNode == null) return 0;

            var txt= revNode.InnerText.Replace("+ Inventory","").Replace("Asking Price:","").Trim();
            return GetNumberFromString(txt, 0);
        }

        private decimal GetMultiple(HtmlDocument doc)
        {
            var revNodes= doc.DocumentNode.SelectNodes("//h6");
            if (revNodes == null) return 0;
            var revNode= revNodes.FirstOrDefault(n => n.InnerText == "MULTIPLE");
            if (revNode == null) return 0;
            var valNode = revNode.ParentNode.ChildNodes.First(n => n.Name=="p");
            return GetNumberFromString(valNode.InnerText,0M);
        }

        private int GetRevenue(HtmlDocument doc)
        {
            var revNodes= doc.DocumentNode.SelectNodes("//h6");
            if (revNodes == null) return 0;
            var revNode = revNodes.FirstOrDefault(n => n.InnerText == "REVENUE");
            if (revNode == null) return 0;
            var valNode = revNode.ParentNode.ChildNodes.First(n => n.Name=="p");
            return GetNumberFromString(valNode.InnerText,0);
        }

        private int GetIncome(HtmlDocument doc)
        {
            var revNodes= doc.DocumentNode.SelectNodes("//h6");
            if (revNodes == null) return 0;
            var revNode= revNodes.FirstOrDefault(n => n.InnerText == "INCOME");
            if (revNode == null) return 0;
            var valNode = revNode.ParentNode.ChildNodes.First(n => n.Name=="p");
            return GetNumberFromString(valNode.InnerText,0);
        }
    }
}
