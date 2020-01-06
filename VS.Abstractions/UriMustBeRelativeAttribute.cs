namespace VS.Abstractions {
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class UriMustBeRelativeAttribute : ValidationAttribute {
        private readonly bool allowNulls;

        public UriMustBeRelativeAttribute(bool allowNulls = false) {
            this.allowNulls = allowNulls;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context) {
            if (value == null) {
                return this.allowNulls ? ValidationResult.Success : new ValidationResult($"Uri cannot be null");
            }

            if (value is Uri uri) {
                return uri.IsAbsoluteUri
                    ? new ValidationResult($"Uri must be relative")
                    : ValidationResult.Success;
            }

            return new ValidationResult($"The property or field must be of type Uri");
        }
    }
}
