namespace Church.Components.Core.Model
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? DateOfBirthDay { get; set; }
        public int? DateOfBirthMonth { get; set; }
        public int? DateOfBirthYear { get; set; }
        public Gender Gender { get; set; }
        public string Occupation { get; set; }
        public TimeZone TimeZone { get; set; }
        public bool IsArchived { get; set; }
    }
}
