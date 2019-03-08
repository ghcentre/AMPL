namespace Ampl.Configuration
{
    public interface IAppConfig
  {
    T Get<T>(string key, bool useResolvers = true);

    void Set<T>(string key, T value);
  }
}