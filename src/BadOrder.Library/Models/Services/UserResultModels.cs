using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Services
{
    public record Created(User Result) : UserResult;
    public record Found(User Result) : UserResult;
    public record All(IEnumerable<User> Result) : UserResult;
    public record EmailInUse(ErrorEntry Result) : UserResult;
    public record InvalidRole(ErrorEntry Result) : UserResult;
    public record AuthenticateFailur(ErrorEntry Result) : UserResult;
    public record AuthenticateSuccess(object Result) : UserResult;
    public record NotFound(ErrorEntry Result) : UserResult;
    public record Updated() : UserResult;
    public record Deleted() :UserResult; 
}
