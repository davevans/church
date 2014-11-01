using System;

namespace Church.Components.Account.Model
{
    public class User
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
    }
}
