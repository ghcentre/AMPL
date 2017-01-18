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
      return GetFirstDisplayAttribute(source)?.Name;
    }

    public static string GetDisplayDescription(this Enum source)
    {
      return GetFirstDisplayAttribute(source)?.Description;
    }
  }
}
