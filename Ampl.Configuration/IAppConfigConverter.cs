using System;

namespace Ampl.Configuration
{
  public interface IAppConfigConverter
  {
    bool CanConvert(Type type);
    object ReadEntity(string entityValue);
    string WriteEntity(object objectValue);
  }
}