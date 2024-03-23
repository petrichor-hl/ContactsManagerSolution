using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string? PersonName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(controller: "Account", action: "IsEmailAlreadyRegistered", ErrorMessage = "Email is already in use")]
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

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
