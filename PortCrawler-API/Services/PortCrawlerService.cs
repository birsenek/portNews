using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            var results = ParseHtml(response);

           // var responseString = await response.Result.ToString();

            return results;
        }

        private string ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var programmerLinks = htmlDoc.DocumentNode.Descendants("tr")
                    .Where(node => node.GetAttributeValue("class", "").Contains("athing")).Take(10).ToList();
            List<HackerNewsItems> newsLinks = new List<HackerNewsItems>();

            foreach (var link in programmerLinks)
            {
                var rank = link.SelectSingleNode(".//span[@class='rank']").InnerText;
                var storyName = link.SelectSingleNode(".//span[@class='titleline']").InnerText;
                var url = link.SelectSingleNode(".//span[@class='titleline']").GetAttributeValue("href", string.Empty);
                var score = link.NextSibling.SelectSingleNode(".//span[@class='score']").InnerText;

                HackerNewsItems item = new HackerNewsItems();
                item.rank = rank.ToString();
                item.title = storyName.ToString();
                item.url = url.ToString();
                item.score = score.ToString();
                newsLinks.Add(item);
            }

            string results = JsonConvert.SerializeObject(newsLinks);
            return results;
        }
    }
}
