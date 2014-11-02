using System;
using Church.Common.Cache;
using Church.Common.Extensions;

namespace Church.Components.Account.Model
{
    public class User : ICacheable
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }

        public string CacheKey 
        {
            get { return "{0}:{1}".FormatWith(GetType().FullName, Id); }
        }
    }
}
