using System;
using System.Globalization;

namespace Ampl.Configuration
{
    public class StringConverter : IAppConfigConverter
  {
    public bool CanConvert(Type type) => type.Equals(typeof(string));
    public object ReadEntity(string entityValue) => entityValue;
    public string WriteEntity(object objectValue) => (string)objectValue;
  }

  public class IntConverter : IAppConfigConverter
  {
    public bool CanConvert(Type type) => type.Equals(typeof(int));
    public object ReadEntity(string entityValue) => int.Parse(entityValue);
    public string WriteEntity(object objectValue) => ((int)objectValue).ToString();
  }

  public class NullableIntConverter : IAppConfigConverter
  {
    public bool CanConvert(Type type) => type.Equals(typeof(int?));
    public object ReadEntity(string entityValue) => string.IsNullOrEmpty(entityValue)
      ? null
      : (int?)int.Parse(entityValue);
    public string WriteEntity(object objectValue) => ((int?)objectValue).ToString();
  }

  public class BoolConverter : IAppConfigConverter
  {
    public bool CanConvert(Type type) => type.Equals(typeof(bool));
    public object ReadEntity(string entityValue) => bool.Parse(entityValue.ToLowerInvariant());
    public string WriteEntity(object objectValue) => ((bool)objectValue).ToString();
  }

  public class NullableBoolConverter : IAppConfigConverter
  {
    public bool CanConvert(Type type) => type.Equals(typeof(bool?));
    public object ReadEntity(string entityValue) => string.IsNullOrEmpty(entityValue)
      ? null
      : (bool?)bool.Parse(entityValue);
    public string WriteEntity(object objectValue) => ((bool?)objectValue).ToString();
  }

  public class DecimalConverter : IAppConfigConverter
  {
    private readonly CultureInfo _convertCulture = new CultureInfo("en-US");
    public bool CanConvert(Type type) => type.Equals(typeof(decimal));
    public object ReadEntity(string entityValue) => decimal.Parse(entityValue, _convertCulture);
    public string WriteEntity(object objectValue) => ((decimal)objectValue).ToString(_convertCulture);
  }

  public class NullableDecimalConverter : IAppConfigConverter
  {
    private readonly CultureInfo _convertCulture = new CultureInfo("en-US");
    public bool CanConvert(Type type) => type.Equals(typeof(decimal?));
    public object ReadEntity(string entityValue) => string.IsNullOrEmpty(entityValue)
      ? null
      : (decimal?)decimal.Parse(entityValue, _convertCulture);
    public string WriteEntity(object objectValue) => ((decimal?)objectValue).HasValue
      ? ((decimal?)objectValue).Value.ToString(_convertCulture)
      : null;
  }
}
