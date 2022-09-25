using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortCrawler_API.Interfaces;

namespace PortCrawler_API.Controllers
{
    [Route("Port")]
    [ApiController]
    public class PortCrawlerController : ControllerBase
    {
        private IPortCrawler _portCrawler;

        public PortCrawlerController(IPortCrawler portCrawler)
        {
            _portCrawler = portCrawler;
        }

        [HttpGet(Name = "GetPortNews")]
        public async Task<string> GetPortNews()
        {
           var response = await _portCrawler.StartPortCrawler();

            return response;
        }
    }

   
}
