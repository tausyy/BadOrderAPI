using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Items.Dtos
{
    public record OrderItem : Item
    {
        public int? Amount { get; init; }
        public bool IsNeeded { get; init; }
    }
}
