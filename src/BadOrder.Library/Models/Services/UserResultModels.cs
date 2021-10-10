using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Models.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
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
    public record EmailInUse(ProblemDetails Result) : UserResult;
    public record InvalidRole(ProblemDetails Result) : UserResult;
    public record AuthenticateFailur(ProblemDetails Result) : UserResult;
    public record AuthenticateSuccess(UserAuthenticated Result) : UserResult;
    public record UserNotFound(ProblemDetails Result) : UserResult;
    public record UserUpdated() : UserResult;
    public record UserDeleted() : UserResult; 
}
