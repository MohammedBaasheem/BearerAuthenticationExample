using System.ComponentModel.DataAnnotations;

namespace BearerAuthentication.DTOs.RequestDtos
{
    public class RegesterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
