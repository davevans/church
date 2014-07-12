using System.Data.Entity.ModelConfiguration;

namespace Church.Components.Core.Repository
{
    public class TimeZoneMappings : EntityTypeConfiguration<Model.Core.TimeZone>
    {
        public TimeZoneMappings()
        {
            ToTable("TimeZone", "Core");
        }
    }
}
