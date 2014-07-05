using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Components.Core.Repository
{
    public class ChurchMappings : EntityTypeConfiguration<Church.Model.Core.Church>
    {
        public ChurchMappings()
        {
            HasKey(x => x.Id);
            HasRequired(x => x.TimeZone)
                .WithMany()
                .HasForeignKey(x => x.TimeZone.Id);
        }
    }
}
