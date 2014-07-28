using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Church.Common.Validation
{
    public class ValidationResults
    {
        public bool IsValid { get; set; }
        public IList<ValidationResult> Errors { get; set; }

        public IEnumerable<string> ErrorStrings
        {
            get { return Errors != null ? Errors.Select(x => x.ErrorMessage).ToList() : Enumerable.Empty<string>(); }
        }
    }
}
