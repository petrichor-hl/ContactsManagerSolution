using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string? PersonName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string? Phone {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
