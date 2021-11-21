using System;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using WebScraper;

namespace WebScraperTests
{
    public class QuietLightListing10834490
    {
        private QuietLightBizForSale doc;
        private QuietLightListingPage webpage;

        [OneTimeSetUp]
        public void Onetimesetup()
        {
            webpage = new QuietLightListingPage(10834490, true);
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
            Assert.AreEqual("Launched in 2018, th", doc.Description.Substring(0,20));
            Assert.IsTrue(doc.Description.Contains("Aside from the crowdfunding"));
            Assert.IsTrue(doc.Description.Contains("The two current owners"));
            Assert.IsTrue(doc.Description.Contains("The valuation takes into consideration"));
        }
        
        [Test]
        public void GetQuietLightListing_TitleIsCorrect()
        {
            Assert.AreEqual( "Luxury Japanese Kitchen Knives | Shopify Based | 144% Trailing 6 Month YOY SDE Growth", doc.Title);
        }
       
        [Test]
        public void GetQuietLightListing_RevenueIsCorrect()
        {
            Assert.AreEqual(167761, doc.Revenue);
        }

        [Test]
        public void GetQuietLightListing_IncomeIsCorrect()
        {
            Assert.AreEqual(42416, doc.Income);
        }

        [Test]
        public void GetQuietLightListing_AskingPriceIsCorrect()
        {
            Assert.AreEqual(125000, doc.AskingPrice);
        }

        [Test]
        public void GetQuietLightListing_MultipleIsCorrect()
        {
            Assert.AreEqual(2.95M, doc.Multiple);
        }
    }
}
