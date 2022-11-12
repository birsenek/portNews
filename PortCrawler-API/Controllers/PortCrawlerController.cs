using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortCrawler_API.Interfaces;
using PortCrawler_API.Model;

namespace PortCrawler_API.Controllers
{
    [Route("PortNews")]
    [ApiController]
    public class PortCrawlerController : ControllerBase
    {
        public IPortCrawler _portCrawler;

        public PortCrawlerController(IPortCrawler portCrawler)
        {
            _portCrawler = portCrawler;
        }

        [HttpGet(Name = "GetPortNews")]
        public async Task<List<PortNewsItems>> GetPortNews()
        {
           var response = await _portCrawler.StartPortCrawler();

            return response;
        }
    }

   
}
