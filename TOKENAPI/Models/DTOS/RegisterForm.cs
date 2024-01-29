using System.ComponentModel.DataAnnotations;

namespace BackEndDevelopment.Models.DTOS
{
    public class RegisterForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool Gender { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; } = "User";
        public bool? Status { get; set; } = false;
        public DateTime? LastLoginTime { get; set; }
        public string? AvatarUrl { get; set; }

    }
}
