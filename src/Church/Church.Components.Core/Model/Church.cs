using System;
using System.Collections.Generic;

namespace Church.Components.Core.Model
{
    public class Church
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeZone TimeZone { get; set; }
        public List<Location> Locations { get; set; }

        public bool IsArchived { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        public Church()
        {
            Locations = new List<Location>();
        }
    }
}
