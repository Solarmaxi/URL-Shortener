using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace URLShortenerAPI.Controllers
{
    public class UrlController
    {
        string shortUrlChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        List<Tuple<string, string>> shortUrlPair = new List<Tuple<string, string>>();
        Random random = new Random();

        [HttpGet]
        public string Encode(string longUrl)
        {
            //Simple validation before we do anything.
            if (string.IsNullOrEmpty(longUrl) || Uri.IsWellFormedUriString(longUrl, UriKind.Absolute) is false)
                return string.Empty;

            //List of Tuples, it's supposed to imitate the database eg. ShortUrl, LongUrl columns
            if (shortUrlPair.Any(v => v.Item2 == longUrl))
                return shortUrlPair.Single(v => v.Item2 == longUrl).Item1;
            else
            {
                string shortUrl = string.Empty;

                while (true) //This might cause infinite loop once we reach 62^8 values
                {
                    shortUrl = CreateKey();

                    if (shortUrlPair.Any(s => s.Item1 == shortUrl) is false) //Ensuring we don't cause collision even though we can technically can store 62^8 values...
                    {
                        shortUrlPair.Add(Tuple.Create(shortUrl, longUrl));
                        return shortUrl;
                    }
                }
            }
            
        }

        [HttpGet]
        public string Decode(string shortUrl)
        {
            return shortUrlPair.SingleOrDefault(s => s.Item1 == shortUrl)?.Item2;
        }

        private string CreateKey()
        {
            return new string(Enumerable.Repeat(shortUrlChars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}