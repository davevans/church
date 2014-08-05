using System;

namespace Church.Components.Core.Model
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Int16? DateOfBirthDay { get; set; }
        public Int16? DateOfBirthMonth { get; set; }
        public Int16? DateOfBirthYear { get; set; }
        public Gender Gender { get; set; }
        public string Occupation { get; set; }
        public TimeZone TimeZone { get; set; }
        public bool IsArchived { get; set; }
    }
}
