namespace Church.Model.Core
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }

        
        public int ChurchId { get; set; }
        public virtual Church Church { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
