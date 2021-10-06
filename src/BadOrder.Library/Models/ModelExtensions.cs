using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Items;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Models.Services;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Models.Users.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models
{
    public static class ModelExtensions
    {
        private const string EnumErrorMagic = "enumError";
        
        private const string EmailInUseError = "Email already in use";
        private const string InvalidRoleError = "Invalid role";
        private const string AuthFailedError = "Email or password provided is invalid";

        private static string NotFoundMessage(string modelName) => $"{modelName} not found";

        public static IActionResult AsActionResult(this OrderResult result) => result switch
        {
            OrderFound order => new OkObjectResult(order.Result),
            OrderCreated order => new OkObjectResult(order.Result),
            OrderNotFound order => new NotFoundObjectResult(order.Result.AsErrorResonse()),
            OrderUnauthorized order => new UnauthorizedObjectResult(order.Result.AsErrorResonse()),
            OrderUpdated or OrderDeleted => new NoContentResult(), 
            _ => new StatusCodeResult(500)
        };

        public static IActionResult AsActionResult(this UserResult result) => result switch
        {

            _ => new StatusCodeResult(500)
        };

        public static IActionResult AsActionResult(this UserResult result, string actionName, string controllerName) => result switch
        {
            EmailInUse request => new ConflictObjectResult(request.Result.AsErrorResonse()),
            InvalidUserRole request => new BadRequestObjectResult(request.Result.AsErrorResonse()),
            
            UserCreated request => 
                new CreatedAtActionResult(actionName, controllerName, new { id = request.Result.Id }, request.Result.AsNewUser()),
           
            _ => new StatusCodeResult(500)
        };

        public static NewUser AsNewUser(this User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            DateAdded = user.DateAdded
        };

        public static User AsUpdatedUser(this User user, NewUserRequest inputUser, string hashedPassword)
        {
            User updatedUser = user with
            {
                Name = inputUser.Name,
                Password = hashedPassword,
                Email = inputUser.Email,
                PhoneNumber = inputUser.PhoneNumber,
                Role = inputUser.Role
            };
            return updatedUser;
        }

        public static ErrorResponse AsErrorResonse(this ErrorEntry error) =>
            new ErrorResponse { Errors = new[] { error } };



        public static IActionResult AuthFailed(this User _)
        {
            var error = new ErrorEntry { Message = AuthFailedError };
            return new BadRequestObjectResult(new ErrorResponse { Errors = new[] { error } });
        }

        public static IActionResult NotFound(this Order model, string id) =>
            new NotFoundObjectResult(new ErrorResponse { Errors = new[] { NotFoundErrorEntry(nameof(Order), id) } });

        public static IActionResult NotFound(this Item model, string id) =>
            new NotFoundObjectResult(new ErrorResponse { Errors = new[] { NotFoundErrorEntry(nameof(Item), id) } });

        public static IActionResult NotFound(this User model, string id) =>
            new NotFoundObjectResult(new ErrorResponse { Errors = new[] { NotFoundErrorEntry(nameof(User), id) } });

        private static ErrorEntry NotFoundErrorEntry(string nameOfModel, string id) => new()
        { 
            Field = nameof(ModelBase.Id), 
            Value = id, 
            Message = NotFoundMessage(nameOfModel) 
        };

        public static ErrorResponse ToErrorResponseModel(this ModelStateDictionary modelState)
        {
            List<ErrorEntry> errors = new();
            foreach (var key in modelState.Keys)
            {
                foreach(var err in modelState[key].Errors)
                {

                    errors.Add( err.isEnumError()
                        ? err.toEnumError()
                        : new ErrorEntry
                        { 
                            Field = key,
                            Value = modelState[key].AttemptedValue,
                            Message = err.ErrorMessage,
                        });
                } 
            }
            return new ErrorResponse() { Errors = errors.ToArray() };
        }

        private static bool isEnumError(this ModelError model) =>
            model.ErrorMessage.StartsWith(EnumErrorMagic);

        private static ErrorEntry toEnumError(this ModelError model)
        {
            var errorParts = model.ErrorMessage.Split('\n');
            return new ErrorEntry()
            {
                Field = errorParts[1],
                Value = errorParts[2],
                Message = errorParts[3]
            };
        }



    }
}
