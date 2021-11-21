using System;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using WebScraper;
using static System.Net.WebRequestMethods;

namespace WebScraperTests
{
    public class QuietLightListingSummaryTest
    {
        private QuietLightListingSummaryPage webpage;

        [OneTimeSetUp]
        public void Onetimesetup()
        {
            webpage = new QuietLightListingSummaryPage(true);
        }

        [Test]
        [Explicit]
        public void DownloadAllListings()
        {
            webpage.DownloadAllListings(5);
        }

        [Test]
        public void GetQuietLightListing_CanParseFile()
        {
            Assert.IsNotNull(webpage);
        }

        [Test]
        public void GetQuietLightListing_FindsAllListings()
        {
            Assert.AreEqual(201, webpage.Summaries.Count);
        }

        [Test]
        public void GetQuietLightListing_FindsListingUnderLetterOfIntent()
        {
            var thisListing = webpage.Summaries.FirstOrDefault(s => s.Id == 10228261);
            Assert.IsNotNull(thisListing);
            Assert.AreEqual(BusinessStatus.LetterOfIntent, thisListing.Status);
        }


        [Test]
        public void GetQuietLightListing_FindsPublicListing()
{
            var thisListing = webpage.Summaries.FirstOrDefault(s => s.Id == 11186124);
            Assert.IsNotNull(thisListing);
            Assert.AreEqual(BusinessStatus.ForSale, thisListing.Status);
        }
    }
}
