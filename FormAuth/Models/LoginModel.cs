using System.ComponentModel.DataAnnotations;

namespace FormAuth.Models
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}