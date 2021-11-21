using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebScraper
{
    public static class ProxyExtension
    {
        public static WebProxy CreateProxy()
        {
            var proxies = new List<Proxy>();

            proxies.Add(new Proxy
            {
                Server = "http://rotator1.x5.net:5",
                UserName = "knowthefacts-3MIN",
                Password = "com"
            });

            proxies.Add(new Proxy
            {
                Server = "http://rotator1.x5.net:5",
                UserName = "knowthefacts-USA",
                Password = "com"
            });

            proxies.Add(new Proxy
            {
                Server = "http://rotator1.x5.net:5",
                UserName = "knowthefacts",
                Password = "com"
            });

            var proxy = proxies[new Random().Next(0, proxies.Count - 1)];

            return new WebProxy(proxy.Server)
            {
                BypassProxyOnLocal = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(proxy.UserName, proxy.Password),
            };

        }

    }
}
