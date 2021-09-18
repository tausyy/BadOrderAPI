using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Users.Dtos
{
    public record LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [MaxLength(52, ErrorMessage = "Must be less than {1} characters")]
        [MinLength(1, ErrorMessage = "Must be at least {1} character")]
        public string Password { get; init; }
    }
}
