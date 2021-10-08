using BadOrder.Library.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Services
{
    public static class ResultExtensions
    {
        public static IActionResult AsActionResult(this ItemResult result, string actionName = default, string controllerName = default) => result switch
        {
            ItemUpdated or ItemDeleted => new NoContentResult(),
            AllItems items => new OkObjectResult(items.Result),
            ItemFound item => new OkObjectResult(item.Result),
            ItemNotFound item => new NotFoundObjectResult(item.Result.AsErrorResonse()),
            ItemCreated request =>
                new CreatedAtActionResult(actionName, controllerName, new { id = request.Result.Id }, request.Result),

            _ => new StatusCodeResult(500)
        };

        public static IActionResult AsActionResult(this OrderResult result) => result switch
        {
            OrderUpdated or OrderDeleted => new NoContentResult(),
            OrderFound order => new OkObjectResult(order.Result),
            OrderCreated order => new OkObjectResult(order.Result),
            OrderNotFound order => new NotFoundObjectResult(order.Result.AsErrorResonse()),
            OrderUnauthorized order => new UnauthorizedObjectResult(order.Result.AsErrorResonse()),

            _ => new StatusCodeResult(500)
        };

        public static IActionResult AsActionResult(this UserResult result, string actionName = default, string controllerName = default) => result switch
        {
            UserUpdated or UserDeleted => new NoContentResult(),
            UserFound request => new OkObjectResult(request.Result),
            AllUsers request => new OkObjectResult(request.Result),
            AuthenticateSuccess request => new OkObjectResult(request.Result),
            EmailInUse request => new ConflictObjectResult(request.Result.AsErrorResonse()),
            InvalidRole request => new BadRequestObjectResult(request.Result.AsErrorResonse()),
            AuthenticateFailur request => new BadRequestObjectResult(request.Result.AsErrorResonse()),
            UserCreated request =>
                new CreatedAtActionResult(actionName, controllerName, new { id = request.Result.Id }, request.Result),

            _ => new StatusCodeResult(500)
        };
    }
}
