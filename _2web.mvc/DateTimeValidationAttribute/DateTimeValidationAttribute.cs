using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Docexchange.Web
{
  public class DateTimeValidationAttribute : ValidationAttribute, IClientValidatable
  {
    #region IClientValidatable Members

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
      yield return new ModelClientValidationRule() {
        ErrorMessage = string.Format(
          BootstrapTemplatesResources.BootstrapTemplatesResources.ClientDataTypeModelValidatorProvider_FieldMustBeDateTime,
          metadata.GetDisplayName()),
        ValidationType = "datetime"
      };
    }

    #endregion

    public override bool IsValid(object value)
    {
      if(value == null)
      {
        return true;
      }

      DateTime time;
      if(!DateTime.TryParse(value.ToString(), out time))
      {
        return false;
      }
      return true;
    }
  }
}
