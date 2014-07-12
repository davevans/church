namespace Church.Model.Core
{
    public class Church
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TimeZoneId { get; set; }
        public virtual TimeZone TimeZone { get; set; }
    }
}
