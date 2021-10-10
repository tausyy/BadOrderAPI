using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Services
{
    public record OrderCreated(Order Result) : OrderResult;
    public record OrderFound(Order Result) : OrderResult;

    public record OrderUnauthorized(ProblemDetails Result) : OrderResult;
    public record OrderNotFound(ProblemDetails Result) : OrderResult;

    public record OrderUpdated() : OrderResult;
    public record OrderDeleted() : OrderResult;
}
