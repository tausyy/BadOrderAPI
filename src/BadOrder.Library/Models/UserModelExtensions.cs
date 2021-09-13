using BadOrder.Library.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models
{
    public static class UserModelExtensions
    {
        public static NewUser AsNewUser(this User user) =>
            new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                DateAdded = user.DateAdded
            };
    }
}
