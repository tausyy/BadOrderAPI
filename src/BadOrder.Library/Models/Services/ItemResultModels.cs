using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Models.Items;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Services
{
    public record ItemCreated(Item Result): ItemResult;
    public record ItemNotFound(ProblemDetails Result): ItemResult;
    public record ItemDeleted(): ItemResult;
    public record ItemUpdated(): ItemResult;
    public record AllItems(IEnumerable<Item> Result) : ItemResult;
    public record ItemFound(Item Result): ItemResult;
}
