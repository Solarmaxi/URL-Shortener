using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using URLShortenerAPI.Filters;

namespace URLShortenerAPI.Controllers
{
    [ActionFilter]
    public class UrlController : ApiController
    {
        string shortUrlChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        List<Tuple<string, string>> shortUrlPair = HttpContext.Current.Application["Links"] as List<Tuple<string, string>>; //Imagine this is DBContext...
        Random random = new Random();

        [HttpPost]
        [Route("api/Url/Encode")]
        public HttpResponseMessage Encode([FromBody] string longUrl)
        {
            //Simple validation before we do anything.
            if (string.IsNullOrEmpty(longUrl) || Uri.IsWellFormedUriString(longUrl, UriKind.Absolute) is false)
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Invalid URL provided!");

            //Check if the longURL is already in the "DB"
            if (shortUrlPair.Any(v => v.Item2 == longUrl))
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, shortUrlPair.Single(v => v.Item2 == longUrl).Item1);
            else
            {
                string shortUrl = string.Empty;

                while (true) //This might cause infinite loop once we reach closer to 62^8 values - TODO - Add some kind of break out of this loop.
                {
                    shortUrl = CreateKey();

                    //Ensuring we don't cause collision even though we can technically can store 62^8 values...
                    if (shortUrlPair.Any(s => s.Item1 == shortUrl) is false) 
                    {
                        shortUrlPair.Add(Tuple.Create(shortUrl, longUrl));
                        return Request.CreateResponse(System.Net.HttpStatusCode.OK, shortUrl);
                    }
                }
            }
        }

        [HttpGet]
        [Route("api/Url/Decode/{shortUrl}")]
        public HttpResponseMessage Decode(string shortUrl)
        {
            if (string.IsNullOrEmpty(shortUrl) is false)
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, shortUrlPair.SingleOrDefault(s => s.Item1 == shortUrl)?.Item2);
            else
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Short URL cannot be empty!");
        }   

        private string CreateKey()
        {
            return new string(Enumerable.Repeat(shortUrlChars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}