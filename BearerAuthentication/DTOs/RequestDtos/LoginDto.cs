using System.ComponentModel.DataAnnotations;

namespace BearerAuthentication.DTOs.RequestDtos
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
