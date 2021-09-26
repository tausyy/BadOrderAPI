using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Orders.Dtos
{
    public record WriteOrder
    {
        [Required]
        public IEnumerable<OrderItem> OrderList { get; init; }
    }
}
