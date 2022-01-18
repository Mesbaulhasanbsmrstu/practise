using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace practise.DTO
{
    public class UpdatePersonDto
    {
        [Required]
        public int PersonId { get; set; }
        public string LastName { get; set; }
        
        public string FirstName { get; set; }
       
        public string Address { get; set; }
       
        public string City { get; set; }
        public IFormFile image { get; set; }
    }
}
