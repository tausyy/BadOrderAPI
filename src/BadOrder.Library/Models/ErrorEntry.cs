using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace BadOrder.Library.Models
{
    public record ErrorEntry
    {
        public string? Field { get; init; } = null;
        public string? Value { get; init; } = null;
        public string Message { get; init; } = default!;
    }
}
