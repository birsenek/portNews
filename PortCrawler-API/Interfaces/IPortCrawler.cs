using PortCrawler_API.Model;

namespace PortCrawler_API.Interfaces
{
    public interface IPortCrawler
    {
        public Task<List<PortNewsItems>> StartPortCrawler();
    }
}
