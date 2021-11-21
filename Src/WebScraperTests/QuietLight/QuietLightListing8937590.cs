using System;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using WebScraper;

namespace WebScraperTests
{
    public class QuietLightListing8937590
    {
        private QuietLightBizForSale doc;
        private QuietLightListingPage webpage;

        [OneTimeSetUp]
        public void Onetimesetup()
        {
            webpage = new QuietLightListingPage(8937590, true);
            doc = webpage.Listing;
        }


        [Test]
        public void GetQuietLightListing_BusinessHasInventoryIsCorrect()
        {
            Assert.AreEqual(true, doc.BusinessHasInventory);
        }


        [Test]
        public void GetQuietLightListing_PriceIncludesInventoryIsCorrect()
        {
            Assert.AreEqual(false, doc.PriceIncludesInventory);
        }

        [Test]
        public void GetQuietLightListing_DescriptionIsCorrect()
        {
            Assert.AreEqual("Launched in 2017, th", doc.Description.Substring(0,20));
            Assert.IsTrue(doc.Description.Contains("The owners target small holes in t"));
            Assert.IsTrue(doc.Description.Contains("The owners sold their first"));
            Assert.IsTrue(doc.Description.Contains("Best of all, the business"));
        }
        
        [Test]
        public void GetQuietLightListing_TitleIsCorrect()
        {
            Assert.AreEqual( "3+ Year Old Amazon FBA Business with 105 ASINs across 5 Brands and 81% YOY SDE Growth", doc.Title);
        }
       
        [Test]
        public void GetQuietLightListing_RevenueIsCorrect()
        {
            Assert.AreEqual(4632655, doc.Revenue);
        }

        [Test]
        public void GetQuietLightListing_IncomeIsCorrect()
        {
            Assert.AreEqual(1670071, doc.Income);
        }

        [Test]
        public void GetQuietLightListing_AskingPriceIsCorrect()
        {
            Assert.AreEqual(6750000, doc.AskingPrice);
        }

        [Test]
        public void GetQuietLightListing_MultipleIsCorrect()
        {
            Assert.AreEqual(4.04M, doc.Multiple);
        }
    }
}
