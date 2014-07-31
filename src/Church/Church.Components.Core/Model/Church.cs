using System.Collections.Generic;

namespace Church.Components.Core.Model
{
    public class Church
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TimeZoneId { get; set; }
        public virtual TimeZone TimeZone { get; set; }

        public ICollection<Location> Locations { get; set; }

        public Church()
        {
            Locations = new List<Location>();
        }
    }
}
