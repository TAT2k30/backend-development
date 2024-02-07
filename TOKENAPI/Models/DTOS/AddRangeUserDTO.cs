namespace BackEndDevelopment.Models.DTOS
{
    public class AddRangeUserDTO
    {
        public string Email { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
