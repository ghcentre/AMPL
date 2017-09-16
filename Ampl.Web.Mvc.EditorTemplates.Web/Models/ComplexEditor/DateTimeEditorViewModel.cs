using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ampl.Web.Mvc.EditorTemplates.Web.Models
{
  public class DateTimeEditorViewModel
  {
    [Display(Name = "DateTime",
             Description = "Please enter the value containing both Date and Time in culture-specific format.",
             Prompt = "Date and Time",
             ShortName = "DateTime")]
    //[DataType(DataType.DateTime)]
    [DateTimeValidation(ValidationType.DateTime)]
    public DateTime DateTimeValue { get; set; }

    [Display(Name = "Nullable DateTime",
             Description = "Please enter the value containing Date and Time or leave blank for null value.",
             Prompt = "Nullable DateTime",
             ShortName = "NullableDateTime")]
    [DataType(DataType.DateTime)]
    [DateTimeValidation(ValidationType.DateTime)]
    public DateTime? NullableDateTime { get; set; }

    [Display(Name = "Date",
             Description = "Please enter a date in culture-specific format.",
             Prompt = "Date and Time",
             ShortName = "Date")]
    [DataType(DataType.Date)]
    public DateTime DateValue { get; set; }

    [Display(Name = "Nullable Date",
             Description = "Please enter a date or leave blank for null value.",
             Prompt = "Nullable DateTime",
             ShortName = "NullableDate")]
    [DataType(DataType.Date)]
    public DateTime? NullableDate { get; set; }
  }
}