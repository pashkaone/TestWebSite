using System.ComponentModel.DataAnnotations;

namespace FormAuth.Models
{
    public class EditModel
    {
        public int Year { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}