using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace Church.Host.Owin.Core.ViewModels.Errors
{
    public class BadRequestViewModel
    {
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; }

        public BadRequestViewModel()
        {
            ErrorMessage = @"There was a problem with your request";
            Errors = new List<string>();
        }

        public BadRequestViewModel(ModelStateDictionary modelState)
        {
            ErrorMessage = @"There was a problem with your request";
            Errors = modelState.Values.SelectMany(e => e.Errors)
                                      .Select(x => x.ErrorMessage)
                                      .ToList();
        }
    }
}