using System.ComponentModel.DataAnnotations;

namespace Shopee.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(20, ErrorMessage = "User Name cannot exceed 20 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
    }
}
