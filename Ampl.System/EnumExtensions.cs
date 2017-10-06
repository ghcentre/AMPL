using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Ampl.System
{
  public static class EnumExtensions
  {
    private static DisplayAttribute GetFirstDisplayAttribute(Enum source)
    {
      return source?
        .GetType()
        .GetTypeInfo()
        .GetDeclaredField(source.ToString())
        //.First()
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
  }
}
