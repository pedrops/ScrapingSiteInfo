using System;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using WebScraper;

namespace WebScraperTests
{
    public class QuietLightListing10109806
    {
        private QuietLightListingPage webpage;
        private QuietLightBizForSale doc;

        [OneTimeSetUp]
        public void Onetimesetup()
        {
            webpage = new QuietLightListingPage(10109806, true);
            doc = webpage.Listing;
        }


        [Test]
        public void GetQuietLightListing_BusinessHasInventoryIsCorrect()
        {
            Assert.IsFalse(doc.BusinessHasInventory.HasValue);
        }


        [Test]
        public void GetQuietLightListing_PriceIncludesInventoryIsCorrect()
        {
            Assert.IsFalse( doc.PriceIncludesInventory.HasValue);
        }

        [Test]
        public void GetQuietLightListing_DescriptionIsCorrect()
        {
            Assert.IsTrue(doc.Description.StartsWith("Started in 1995, this outdoor sporting goods "));
            Assert.IsTrue(doc.Description.Contains("The company currently operates out"));
            Assert.IsTrue(doc.Description.Contains("Until recently, the company focused"));
            Assert.IsTrue(doc.Description.Contains("This is an exceptional company"));
        }
        
        [Test]
        public void GetQuietLightListing_TitleIsCorrect()
        {
            Assert.AreEqual( "25 Year Old Outdoor Sporting Goods Ecommerce Business", doc.Title);
        }
       
        [Test]
        public void GetQuietLightListing_RevenueIsCorrect()
        {
            Assert.AreEqual(22523477, doc.Revenue);
        }

        [Test]
        public void GetQuietLightListing_IncomeIsCorrect()
        {
            Assert.AreEqual(2028466, doc.Income);
        }

        [Test]
        public void GetQuietLightListing_AskingPriceIsCorrect()
        {
            Assert.AreEqual(0, doc.AskingPrice);
        }

        [Test]
        public void GetQuietLightListing_MultipleIsCorrect()
        {
            Assert.AreEqual(0, doc.Multiple);
        }
    }
}
