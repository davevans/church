using System.Data.Entity.ModelConfiguration;

namespace Church.Components.Core.Repository.ModelMappings
{
    public class ChurchMappings : EntityTypeConfiguration<Model.Core.Church>
    {
        public ChurchMappings()
        {
            ToTable("Church", "Core");
            HasKey(x => x.Id);
            HasRequired(x => x.TimeZone)
                .WithMany()
                .HasForeignKey(x => x.TimeZoneId);
        }
    }
}
