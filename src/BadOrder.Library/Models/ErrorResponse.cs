using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models
{
    public record ErrorResponse
    {
        public string Error { get; init; }
    }
}
