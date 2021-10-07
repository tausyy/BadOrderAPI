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

        public static IActionResult AsActionResult(this UserResult result, string actionName = default, string controllerName = default) => result switch
        {
            EmailInUse request => new ConflictObjectResult(request.Result.AsErrorResonse()),
            InvalidRole request => new BadRequestObjectResult(request.Result.AsErrorResonse()),
            AuthenticateFailur request => new BadRequestObjectResult(request.Result.AsErrorResonse()),
            AuthenticateSuccess request => new OkObjectResult(request.Result),
            All request => new OkObjectResult(request.Result),
            Found request => new OkObjectResult(request.Result),
            Updated or Deleted => new NoContentResult(),
            Created request =>  
                new CreatedAtActionResult(actionName, controllerName, new { id = request.Result.Id }, request.Result),
           
            _ => new StatusCodeResult(500)
        };

        public static ErrorResponse AsErrorResonse(this ErrorEntry error) =>
            new() { Errors = new[] { error } };

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

                    errors.Add( err.IsEnumError()
                        ? err.ToEnumError()
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

        private static bool IsEnumError(this ModelError model) =>
            model.ErrorMessage.StartsWith(EnumErrorMagic);

        private static ErrorEntry ToEnumError(this ModelError model)
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
