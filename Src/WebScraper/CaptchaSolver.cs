using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebScraper
{
    public class CaptchaSolver
    {

        public CookieContainer CookieJar;
        public string CaptchaResponse { get; set; }
        public int TimesUsingSameCaptcha { get; set; }



        public CaptchaSolver( )
        {   
            TimesUsingSameCaptcha = 0;
        }


        /// <summary>
        /// Solve the captcha v2/v3 using site-key value and page url
        /// </summary>
        /// <param name="captchaPage"></param>
        /// <param name="siteKey"></param>
        /// <param name="source"></param>
        public async Task SolveCaptchaAsync(string captchaPage, string siteKey, string source)
        {
            bool stillAValidCaptcha = TimesUsingSameCaptcha < 1 && !string.IsNullOrEmpty(CaptchaResponse);
            if (stillAValidCaptcha)
            {
                TimesUsingSameCaptcha++;
            }
            else
            {
                string response = await GetCaptchaAsync(captchaPage, siteKey, source);
                CaptchaResponse = response;
                TimesUsingSameCaptcha = 1;
            }
        }

        private async Task<string> GetCaptchaAsync(string websiteUrl, string siteId, string source)
        {
            int innerRetries = 30;
            int innerErrors = 0;
            while (true)
            {
                try
                {
                    string captchaSolution = await SolveCaptchaV2(source, siteId, websiteUrl);
                    return captchaSolution;
                }
                catch (WebException ex)
                {
                    innerErrors++;
                    using (Stream streamer = ex.Response.GetResponseStream())
                    using (StreamReader streamReader = new StreamReader(streamer ?? throw new InvalidOperationException()))
                    {
                        string Message = streamReader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    innerErrors++;
                    if (innerErrors > innerRetries)
                        throw new Exception();
                }
            }
        }

        public async Task<string> SolveCaptchaV2(string sourceId, string siteKey, string pageUrl)
        {
            int innerRetries = 10;
            int innerErrors = 0;
            while (true)
            {
                try
                {
                    string captchaSolution = await SolveCaptchaV2(siteKey, pageUrl);
                    return captchaSolution;
                }
                catch (Exception e)
                {
                    innerErrors++;
                    if (innerErrors > innerRetries)
                        throw new Exception();
                }
            }
        }

        private async Task<string> SolveCaptchaV2(string siteKey, string pageUrl)
        {
            string captchaId = await GetHtml("http://2captcha.com/in.php?key=0bac17d2118413298ce5501b495a8e1b&method=userrecaptcha&googlekey=" + siteKey + "&pageurl=" + pageUrl, "");

            Console.WriteLine("Solving Captcha");
            var tries = 0;
            while (true)
            {
                if(tries > 10)
                    throw new Exception("Captcha Not Ready");

                await Task.Delay(new Random().Next(10 * 1000, 20 * 1000));
                var captcha = await GetHtml("http://2captcha.com/res.php?key=0bac17d2118413298ce5501b495a8e1b&action=get&id=" + captchaId.Substring(3, captchaId.Length - 3), "");

                if (captcha == "CAPCHA_NOT_READY")
                {
                    tries++;
                    continue;
                }

                if (captcha.Contains("OK|"))
                {
                    string tempR = captcha.Replace("OK|", "");
                    captcha = tempR;
                }

                return captcha;
            }
            
        }


        public Task<string> GetHtml(string url, string referer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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
            

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var html = reader.ReadToEnd();
                        return Task.FromResult(html);
                    }
                }
            }

        }

    }
}
