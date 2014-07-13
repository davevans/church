﻿using Church.Common.Database;
using System.Data.Entity;
using Church.Components.Core.Repository.ModelMappings;
using ModelCore = Church.Model.Core;

namespace Church.Components.Core.Repository
{
    public class CoreContext : BaseContext<CoreContext>
    {
        public DbSet<ModelCore.Church> Churches { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new ChurchMappings());
            builder.Configurations.Add(new TimeZoneMappings());
            builder.Configurations.Add(new LocationMappings());
            builder.Configurations.Add(new AddressMappings());
        }
    }
}
