using BadOrder.Library.Models.Items;
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
        private const string UserNotFoundError = "User not found";
        private const string EmailInUseError = "Email already in use";
        private const string InvalidRoleError = "Invalid role";

        private const string ItemNotFoundError = "Item not found";

        private const string EnumErrorMagic = "enumError";

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

        public static IActionResult NotFound(this User user, string id)
        {
            var error = new ErrorEntry { Field = nameof(user.Id), Value = id, Message = UserNotFoundError };
            return new NotFoundObjectResult(new ErrorResponse { Errors = new[] { error } });
        }

        public static IActionResult EmailInUse(this WriteUser user)
        {
            var error = new ErrorEntry { Field = nameof(user.Email), Value = user.Email, Message = EmailInUseError };
            return new ConflictObjectResult(new ErrorResponse { Errors = new[] { error } });
        }

        public static IActionResult InvalidRole(this WriteUser user)
        {
            var error = new ErrorEntry { Field = nameof(user.Role), Value = user.Role, Message = InvalidRoleError };
            return new BadRequestObjectResult(new ErrorResponse { Errors = new[] { error } });
        }

        public static IActionResult NotFound(this Item item, string id)
        {
            var error = new ErrorEntry { Field = nameof(item.Id), Value = id, Message = ItemNotFoundError };
            return new NotFoundObjectResult(new ErrorResponse { Errors = new[] { error } });

        }

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
