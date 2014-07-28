using System;

namespace Church.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateChildAttribute : Attribute
    {
        public bool Required { get; set; }
        public string ErrorMessage { get; set; }

        public ValidateChildAttribute(bool required, string errorMessage)
        {
            Required = required;
            ErrorMessage = errorMessage;
        }

        public ValidateChildAttribute()
            : this(true, "Is required.")
        {

        }
    }
}