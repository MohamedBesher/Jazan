using System.ComponentModel.DataAnnotations;

namespace Saned.Jazan.Api.Models
{
    public class UserViewModel
    {
      
        public string UserId { get; set; }

        public string PhotoUrl { get; set; }

        public string UserName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}