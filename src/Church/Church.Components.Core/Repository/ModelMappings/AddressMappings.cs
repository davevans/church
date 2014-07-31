using System.Data.Entity.ModelConfiguration;
using Church.Components.Core.Model;

namespace Church.Components.Core.Repository.ModelMappings
{
    public class AddressMappings : EntityTypeConfiguration<Address>
    {
        public AddressMappings()
        {
            ToTable("Address", "Core");
            HasKey(x => x.Id);
            //HasRequired(x => x.Street1);
        }
    }
}
