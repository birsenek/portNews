using Microsoft.AspNetCore.Mvc;
using PortCrawler_API.Interfaces;
using System.Net;

namespace PortCrawler_API.Services
{
    public class PortCrawlerService : IPortCrawler
    {
        //public async Task<string> StartPortCrawler()
        //{
        //    string fullUrl = "https://news.ycombinator.com";
        //    var response = CallUrl(fullUrl).Result;
        //    return response;
        //}

        public async Task<string> StartPortCrawler()
        {
            string fullUrl = "https://news.ycombinator.com/";
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetStringAsync(fullUrl).ConfigureAwait(false);
            
           // var responseString = await response.Result.ToString();

            return response;
        }
    }
}
