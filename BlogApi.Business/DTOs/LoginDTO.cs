using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Business.DTOs
{
    public class LoginDTO
    {
        [Required,EmailAddress,DefaultValue("test@gmail.com")]
        public string Email { get; set; } = string.Empty;

        [Required,MinLength(6), DefaultValue("test123")]
        public string Password { get; set; } =string.Empty;
    }
}
