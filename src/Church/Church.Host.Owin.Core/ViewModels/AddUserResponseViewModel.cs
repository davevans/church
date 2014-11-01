using System;

namespace Church.Host.Owin.Core.ViewModels
{
    public class AddUserResponseViewModel
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public DateTime Created { get; set; }
    }
}