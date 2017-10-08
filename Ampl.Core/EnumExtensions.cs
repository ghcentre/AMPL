using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.System
{
  public static class EnumExtensions
  {
    private static DisplayAttribute GetFirstDisplayAttribute(Enum source)
    {
      //return source?.GetType()
      //              .GetTypeInfo()
      //              .GetDeclaredField(source.ToString())
      //              .GetCustomAttributes(typeof(DisplayAttribute), false)
      //              .Select(x => x as DisplayAttribute)
      //              .FirstOrDefault();

      return source.GetType()
                   .GetMember(source.ToString())
                   .FirstOrDefault()
                   .GetCustomAttributes(typeof(DisplayAttribute), false)
                   .Select(x => x as DisplayAttribute)
                   .FirstOrDefault();
    }

    public static string GetDisplayName(this Enum source)
    {
      return GetFirstDisplayAttribute(source)?.GetName();
    }

    public static string GetDisplayDescription(this Enum source)
    {
      return GetFirstDisplayAttribute(source)?.GetDescription();
    }

    public static T ParseValue<T>(this T enumeration,
                                  string source,
                                  bool ignoreCase = false)
      where T : struct, IComparable, IFormattable
    {
      return (T)Enum.Parse(typeof(T), source, ignoreCase);
    }

    public static T? ParseAsNullable<T>(this T enumeration,
                                        string source,
                                        bool ignoreCase = false)
      where T : struct, IComparable, IFormattable
    {
      bool success = Enum.TryParse<T>(source, ignoreCase, out T result);
      if(!success)
      {
        return null;
      }
      return result;
    }
  }
}
