using System.Collections.Generic;

namespace Church.Host.Core.ViewModels
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
