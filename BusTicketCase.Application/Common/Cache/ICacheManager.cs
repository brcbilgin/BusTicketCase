namespace BusTicketCase.Application.Common.Cache
{
    public interface ICacheManager
    {
        Task<T> Get<T>(string key);
        Task Set<T>(string key, T value);
    }
}
