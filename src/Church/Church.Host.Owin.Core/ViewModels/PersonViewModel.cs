using System;
using System.ComponentModel.DataAnnotations;
using Church.Components.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Church.Host.Owin.Core.ViewModels
{
    public class PersonViewModel
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        public Int16? DateOfBirthDay { get; set; }
        public Int16? DateOfBirthMonth { get; set; }
        public Int16? DateOfBirthYear { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender is required.")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }

        public string Occupation { get; set; }
        public TimeZoneViewModel TimeZone { get; set; }
        
    }
}