using System.Collections.Generic;

namespace practise.DTO
{
    public class PersonDetails
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Image { get; set; }
        public List<ProductDetails> products { get; set; }
    }
}
