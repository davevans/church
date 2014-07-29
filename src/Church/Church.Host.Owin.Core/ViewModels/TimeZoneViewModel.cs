using System.ComponentModel.DataAnnotations;

namespace Church.Host.Owin.Core.ViewModels
{
    public class TimeZoneViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}