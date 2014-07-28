using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Church.Common.Validation
{
    public static class Validator
    {

        private class ValidationProperty
        {
            public bool IsClassValidator;
            public PropertyInfo PropertyInfo;
            public ValidateChildAttribute Child;
            public ValidationAttribute[] Validations;
        }

        private static readonly ConcurrentDictionary<Type, List<ValidationProperty>> Cache = new ConcurrentDictionary<Type, List<ValidationProperty>>();
        public static bool Validate(object toValidate, IList<ValidationResult> results)
        {
            var ctx = new ValidationContext(toValidate);
            return Validate(toValidate, ctx, results);
        }

        public static ValidationResults Validate(object o)
        {
            var validationResults = new ValidationResults();
            var list = validationResults.Errors = new List<ValidationResult>();
            validationResults.IsValid = Validate(o, list);
            return validationResults;
        }

        private static ValidationAttribute[] GetPropertyValidator(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }

        public static bool Validate(object instance, ValidationContext validationContext, IList<ValidationResult> validationResults)
        {
            bool isError = false;
            List<ValidationProperty> validationProperties = null;

            var type = instance.GetType();

            if (!Cache.TryGetValue(type, out validationProperties))
            {
                //not found in cache
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                validationProperties = properties.Select(p => new ValidationProperty
                {
                    PropertyInfo = p,
                    Child =  (ValidateChildAttribute)p.GetCustomAttributes(typeof(ValidateChildAttribute), true).FirstOrDefault(),
                    Validations = GetPropertyValidator(p)
                }).ToList();


                var classLevelValidators = (ValidationAttribute[])type.GetCustomAttributes(typeof (ValidationAttribute), true);
                if (classLevelValidators.Any())
                {
                    validationProperties.Add(new ValidationProperty
                    {
                        IsClassValidator = true,
                        PropertyInfo = null,
                        Child = null,
                        Validations = classLevelValidators
                    });
                }

                Cache.TryAdd(type, validationProperties.Any() ? validationProperties : null);
            }

            foreach (var validationProperty in validationProperties)
            {
                object objectValue = validationProperty.IsClassValidator
                    ? instance
                    : validationProperty.PropertyInfo.GetValue(instance, null);

                if (validationProperty.Validations != null)
                {
                    foreach (var validation in validationProperty.Validations)
                    {
                        var result = validation.GetValidationResult(objectValue, validationContext);
                        if (result != null)
                        {
                            isError = true;
                            validationResults.Add(result);
                            break;
                        }
                    }
                }

                if (validationProperty.Child != null)
                {
                    if (objectValue != null && validationProperty.Child.Required)
                    {
                        isError = true;
                        validationResults.Add(new ValidationResult(validationProperty.Child.ErrorMessage));
                    }else if (objectValue != null)
                    {

                        validationContext = new ValidationContext(objectValue, null, null);
                        var innerIsValid = Validate(objectValue, validationContext, validationResults);
                        if (!innerIsValid)
                        {
                            isError = true;
                        }
                    }
                }
            }

            return !isError;
        }


    }
}
