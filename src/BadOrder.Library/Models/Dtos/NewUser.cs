using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Dtos
{
    public record NewUser
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Role { get; init; }
        public DateTimeOffset DateAdded { get; init; }
    }
}
