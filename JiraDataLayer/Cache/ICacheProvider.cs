namespace JiraDataLayer.Cache
{
    public interface ICacheProvider
    {
        ICache<T> CreateCache<T>() where T : class;

        ICache<T[]> CreateArrayCache<T>() where T : class;
    }
}
