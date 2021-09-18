using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Users.Dtos
{
    public record WriteUser
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Must be less than {1} characters")]
        public string Name { get; init; }
        
        [Required]
        [MaxLength(26, ErrorMessage = "Must be less than {1} characters")]
        [MinLength(8, ErrorMessage = "Must be at least {1} characters")]
        public string Password { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; init; }

        [Required]
        [MaxLength(15, ErrorMessage = "Must be less than {1} characters")]
        [MinLength(4, ErrorMessage = "Must be at least {1} characters")]
        public string Role { get; set; }
    }
}
