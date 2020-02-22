using Ampl.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ampl.Web.Mvc
{
    public enum ValidationType
    {
        DateTime,
        Time
    }

    public class DateTimeValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly ValidationType _validationType;

        public DateTimeValidationAttribute(ValidationType validationType) : base()
        {
            _validationType = validationType;
        }

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            if(_validationType == ValidationType.DateTime)
            {
                yield return new ModelClientValidationRule() {
                    ErrorMessage = string.Format(Messages.FieldMustBeADateTime, metadata.GetDisplayName()),
                    ValidationType = "datetime"
                };
            }
            else if(_validationType == ValidationType.Time)
            {
                yield return new ModelClientValidationRule() {
                    ErrorMessage = string.Format(Messages.FieldMustBeATime, metadata.GetDisplayName()),
                    ValidationType = "time"
                };
            }
        }

        #endregion

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }

            return value.ToString().ToNullableDateTime().HasValue;
        }
    }
}
