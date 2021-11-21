using System;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using WebScraper;

namespace WebScraperTests
{
    public class Class1
    {
        [Test]
        public void GetHtml()
        {
            var x = new WebPage();
            //x.LoadFromWeb("https://quietlight.com/listings/10834490","").Wait();
            x.LoadFromWeb("https://en.wikipedia.org/wiki/List_of_programmers","").Wait();
            Assert.IsTrue(x.Doc.Text.Contains("List of programmers"));

            var programmerLinks = x.Doc 
                .DocumentNode.Descendants("li")
                .Where(node => !node.GetAttributeValue("class", "").Contains("tocsection")).ToList();
            Assert.IsNotNull(programmerLinks);
            Assert.IsTrue(programmerLinks.Count > 0);

        }
        
        [Test]
        [Explicit]
        public void SaveToFile()
        {
            var x = new WebPage();
            x.LoadFromWeb("https://quietlight.com/listings/10834490","").Wait();
            x.SaveToFile("d:\\solutions\\woodscraper\\DownloadedPages\\QuietLight10834490.html");
        }
    }
}
