using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models
{
  public class NumericEditorViewModel
  {
    [Display(Name = "Short Value",
             Description = "Please enter the integer value (positive or negative) from -32768 to 32767",
             Prompt = "Example: 0",
             ShortName = "Short Val")]
    public short ShortValue { get; set; }

    [Display(Name = "Nullable Short Value",
             Description = "Please enter the integer value or leave blank for null value.",
             Prompt = "Example: 0 or nothing",
             ShortName = "NullableIntVal1")]
    public short? NullableShortValue { get; set; }

    [Display(Name = "Unsigned Short Value",
             Description = "Please enter the unsigned integer value from 0 to 65535",
             Prompt = "Example: 0",
             ShortName = "UInt Val")]
    public ushort UShortValue { get; set; }

    [Display(Name = "Nullable Unsigned Short Value",
             Description = "Please enter the integer value or leave blank for null value.",
             Prompt = "Example: 0 or nothing",
             ShortName = "Nullable UInt Val")]
    public ushort? NullableUShortValue { get; set; }

    [Display(Name = "Integer Value",
             Description = "Please enter the integer value (positive or negative) from -2147483648 to 2147483647",
             Prompt = "Example: 0",
             ShortName = "Int Val")]
    public int IntValue { get; set; }

    [Display(Name = "Nullable Integer Value",
             Description = "Please enter the integer value or leave blank for null value.",
             Prompt = "Example: 0 or nothing",
             ShortName = "Nullable Int Val")]
    public int? NullableIntValue { get; set; }

    [Display(Name = "Unsigned Integer Value",
             Description = "Please enter the unsigned integer value from 0 to 4294967295",
             Prompt = "Example: 0",
             ShortName = "UInt Val")]
    public uint UIntValue { get; set; }

    [Display(Name = "Nullable Unsigned Integer Value",
             Description = "Please enter the integer value or leave blank for null value.",
             Prompt = "Example: 0 or nothing",
             ShortName = "Nullable UInt Val")]
    public uint? NullableUIntValue { get; set; }

    [Display(Name = "Long Value",
             Description = "Please enter the integer value (positive or negative) from -9223372036854775808 to 9223372036854775807",
             Prompt = "Example: 0",
             ShortName = "Long Val")]
    public long LongValue { get; set; }

    [Display(Name = "Nullable Long Value",
             Description = "Please enter the integer value or leave blank for null value.",
             Prompt = "Example: 0 or nothing",
             ShortName = "Nullable Long Val")]
    public long? NullableLongValue { get; set; }

    [Display(Name = "Unsigned Long Value",
             Description = "Please enter the integer value from 0 to 18446744073709551615",
             Prompt = "Example: 0",
             ShortName = "ULong Val")]
    public ulong ULongValue { get; set; }

    [Display(Name = "Nullable Unsigned Long Value",
             Description = "Please enter the integer value or leave blank for null value.",
             Prompt = "Example: 0 or nothing",
             ShortName = "Nullable ULong Val")]
    public ulong? NullableULongValue { get; set; }
  }
}