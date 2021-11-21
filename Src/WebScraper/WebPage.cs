using System;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Text;

namespace WebScraper
{
    public class WebPage 
    {
        public CookieContainer CookieJar;

        public HtmlDocument Doc;
        public bool LoadedFromFile { get; set; }

        public virtual void SaveToFile(string fullFilePath)
        {
            File.WriteAllTextAsync(fullFilePath, Doc.Text);
        }

        public virtual void LoadFromFile(string fullFilePath)
        {
            if (!File.Exists(fullFilePath)) throw new ArgumentOutOfRangeException();
            string html = File.ReadAllText(fullFilePath, Encoding.UTF8);
            Doc = new HtmlDocument();
            Doc.LoadHtml(html);
            LoadedFromFile = true;
        }

        public virtual void LoadFromString(string html)
        {
            Doc = new HtmlDocument();
            Doc.LoadHtml(html);
        }

        protected decimal GetNumberFromString(string txt, decimal defaultVal)
        {
            txt = CleanNumberString(txt);
            if (Decimal.TryParse(txt, out var decVal)) return decVal;
            return defaultVal;
        }

        protected int GetNumberFromString(string txt, int defaultVal)
        {
            txt = CleanNumberString(txt);
            if (int.TryParse(txt, out var decVal)) return decVal;
            return defaultVal;
        }

        private string CleanNumberString(string txt)
        {
            txt = txt.Replace("x", "").Replace(",","").Replace("$","");

            return txt.Trim();
        }

    public async Task LoadFromWeb(string url, string referer)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.KeepAlive = true;
            request.CookieContainer = CookieJar;
            request.AllowAutoRedirect = true;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (referer.Length > 0) request.Referer = referer;
            

            using (var response = (HttpWebResponse)(await request.GetResponseAsync()))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var html = await reader.ReadToEndAsync();
                        LoadFromString(html);
                    }
                }
            }

        }

    }
}
