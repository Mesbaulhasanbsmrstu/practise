using Microsoft.AspNetCore.Http;
using practise.Validations;
using System.ComponentModel.DataAnnotations;

namespace practise.DTO
{
    public class Persondto
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
       // [FileSizeValidator(4)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile image { get; set; }
    }
}
