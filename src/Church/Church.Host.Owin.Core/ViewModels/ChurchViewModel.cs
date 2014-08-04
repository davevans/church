using System;
using System.ComponentModel.DataAnnotations;

namespace Church.Host.Owin.Core.ViewModels
{
    public class ChurchViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "TimeZone is requried.")]
        public TimeZoneViewModel TimeZone { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
