using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Items;
using BadOrder.Library.Models.Orders;
using BadOrder.Library.Models.Services;
using BadOrder.Library.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace BadOrder.Library.Models
{
    public static class ModelExtensions
    {
        private const string EnumErrorMagic = "enumError";

        public static ErrorResponse AsErrorResonse(this ErrorEntry error) =>
            new() { Errors = new[] { error } };

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
