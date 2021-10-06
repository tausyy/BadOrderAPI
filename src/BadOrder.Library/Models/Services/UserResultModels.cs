using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Services
{
    public record UserCreated(User Result) : UserResult;
    public record EmailInUse(ErrorEntry Result) : UserResult;
    public record InvalidUserRole(ErrorEntry Result) : UserResult;
    public record UserNotFound(ErrorEntry Result) : UserResult;
    public record AllUsers(IEnumerable<User> Result) : UserResult;
    public record UserFound(User Result) : UserResult;
}
