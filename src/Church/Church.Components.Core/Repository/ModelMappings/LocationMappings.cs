using System.Data.Entity.ModelConfiguration;
using Church.Model.Core;

namespace Church.Components.Core.Repository.ModelMappings
{
    public class LocationMappings : EntityTypeConfiguration<Location>
    {
        public LocationMappings()
        {
            ToTable("Location", "Core");
            HasKey(x => x.Id);
            //HasRequired(x => x.Name);
            
            HasRequired(x => x.Address)
                .WithMany()
                .HasForeignKey(x => x.AddressId);
        }
    }
}
