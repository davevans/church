using System.Collections.Generic;

namespace Church.Host.Owin.Core.ViewModels
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
    }
}