using System.ComponentModel.DataAnnotations;

namespace Church.Host.Owin.Core.ViewModels
{
    public class AddUserRequestViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "PersonId is required.")]
        public long PersonId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}