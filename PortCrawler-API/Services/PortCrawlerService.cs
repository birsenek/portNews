using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PortCrawler_API.Interfaces;
using PortCrawler_API.Model;
using System.Net;

namespace PortCrawler_API.Services
{
    public class PortCrawlerService : IPortCrawler
    {
        public readonly IConfiguration _configuration;

        public PortCrawlerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<PortNewsItems>> StartPortCrawler()
        {
            List<string> URLToScan = _configuration.GetSection("SitesPortuarios:URLs").Get<List<string>>();
            string HtmlResponse = string.Empty;
            List<PortNewsItems> results = new List<PortNewsItems>();
            foreach (string url in URLToScan)
            {
                string fullUrl = url;
                HttpClient client = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
                client.DefaultRequestHeaders.Accept.Clear();
                HtmlResponse = await client.GetStringAsync(fullUrl).ConfigureAwait(false);
                results.AddRange(ParseHtml(HtmlResponse));
            }

            return results;
        }

        public List<PortNewsItems> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            List<PortNewsItems> results = new List<PortNewsItems>();
            htmlDoc.LoadHtml(html);

            if (html.Contains("jornalportuario"))
            {
                results.AddRange(ParseJornalPortuario(htmlDoc));
            }
                return results;

        }

        public List<PortNewsItems> ParseJornalPortuario(HtmlDocument htmlDoc)
        {
            string results = string.Empty;
            var programmerLinks = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Contains("o-box rounded p-0")).Take(10).ToList();

            List<PortNewsItems> newsLinks = new List<PortNewsItems>();

            foreach (var link in programmerLinks)
            {
                var storyName = link.SelectSingleNode(".//h2[@class='o-title-slider']").InnerText;
                var url = link.SelectSingleNode(".//a[@href]").GetAttributeValue("href", string.Empty);

                PortNewsItems item = new PortNewsItems();

                item.title = storyName.ToString();
                item.url = url.ToString();
                newsLinks.Add(item);
            }

            var filteredList = new List<PortNewsItems>();

            foreach (var item in newsLinks)
            {
                if (item.title.Contains("Port") || item.title.Contains("Porto") || item.title.Contains("Navio") || item.title.Contains("Ship"))
                {
                    filteredList.Add(item);
                }
            }

            return filteredList;
        }
    }
}
