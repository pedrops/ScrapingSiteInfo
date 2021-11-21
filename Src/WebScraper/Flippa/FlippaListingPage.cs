using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace WebScraper.Flippa
{
    public class FlippaListingPage: WebPage
    {
        public FlippaBizForSale Listing { get; set; }

        public FlippaListingPage(int num, bool loadIfNotFound = false)
        {
            Listing = new FlippaBizForSale();
            Listing.Status = BusinessStatus.ForSale;
            Listing.ListingNum = num;
            Listing.FileName = $"D:\\Solutions\\WoodScraper\\DownloadedPages\\Flippa{num}.html";
            Listing.Url = $"https://flippa.com/10735678-saas-internet";
            if (File.Exists(Listing.FileName)) LoadFromFile(Listing.FileName);
            else if (loadIfNotFound)
            {
                LoadFromWeb(Listing.Url, "").Wait();
                SaveToFile(Listing.FileName);
            }
        }


    }
}
