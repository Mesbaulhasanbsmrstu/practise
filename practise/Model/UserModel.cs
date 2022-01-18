using System.ComponentModel.DataAnnotations;

namespace practise.Model
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string EmailAddress { get; set; }
    }
}
