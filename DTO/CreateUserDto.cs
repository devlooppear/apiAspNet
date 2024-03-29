using System.ComponentModel.DataAnnotations;

namespace apiAspNet.Models
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; } 

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string ?Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        public string ?Telephone { get; set; }

        public string ?SecondTelephone { get; set; }
    }
}
