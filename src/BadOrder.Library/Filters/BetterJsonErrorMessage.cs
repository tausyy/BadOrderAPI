using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Filters
{
    public class BetterJsonErrorMessage : IResultFilter
    {
        private const string EnumErrorMagic = "enumError";

        public void OnResultExecuted(ResultExecutedContext context) { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is not BadRequestObjectResult result) return;
            if (result.Value is not ValidationProblemDetails details) return;           
            
            IfJsonErrorThenFormat(details);
        }

        private static void IfJsonErrorThenFormat(ValidationProblemDetails details)
        {
            var errors = details.Errors.ToList();
            foreach(var error in errors)
            {
                if (IsJsonError(error))
                {
                    ReplaceJsonErrorMessage(details);
                }

                if (IsEnumError(error))
                {
                    ReplaceEnumErrorMessage(details, error.Key);
                }
            }
        }

        private static bool IsJsonError(KeyValuePair<string, string[]> error) => error.Key == "$";
        private static bool IsEnumError(KeyValuePair<string, string[]> error) => error.Value[0].StartsWith(EnumErrorMagic);

        private static void ReplaceJsonErrorMessage(ValidationProblemDetails details)
        {
            details.Errors.Remove("$");
            details.Errors.Add("json", new[] { "Cannot process request because of malformed JSON" });
        }

        private static void ReplaceEnumErrorMessage(ValidationProblemDetails details, string key)
        {
            var enumError = details.Errors[key][0].Split('\n');
            details.Errors.Remove(key);
            details.Errors.Add(enumError[1], new[] { enumError[2] });
        }

    }
}
