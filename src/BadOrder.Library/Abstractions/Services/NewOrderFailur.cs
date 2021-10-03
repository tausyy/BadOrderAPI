using BadOrder.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Abstractions.Services
{
    public record NewOrderFailur : OrderResult
    {
        public ErrorEntry Result { get; init; }
        public NewOrderFailur(ErrorEntry errorEntry) => Result = errorEntry;
    }
}
