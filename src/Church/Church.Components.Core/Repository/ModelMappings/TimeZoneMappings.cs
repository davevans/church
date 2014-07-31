using System.Data.Entity.ModelConfiguration;

namespace Church.Components.Core.Repository.ModelMappings
{
    public class TimeZoneMappings : EntityTypeConfiguration<Model.TimeZone>
    {
        public TimeZoneMappings()
        {
            ToTable("TimeZone", "Core");
        }
    }
}
