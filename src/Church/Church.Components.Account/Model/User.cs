using System;

namespace Church.Components.Account.Model
{
    public class User
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
    }
}
