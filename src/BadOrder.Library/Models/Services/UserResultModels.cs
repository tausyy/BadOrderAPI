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
    public record UserFound(User Result) : UserResult;
    public record AllUsers(IEnumerable<User> Result) : UserResult;
    public record EmailInUse(ErrorEntry Result) : UserResult;
    public record InvalidRole(ErrorEntry Result) : UserResult;
    public record AuthenticateFailur(ErrorEntry Result) : UserResult;
    public record AuthenticateSuccess(object Result) : UserResult;
    public record NotFound(ErrorEntry Result) : UserResult;
    public record UserUpdated() : UserResult;
    public record UserDeleted() :UserResult; 
}
