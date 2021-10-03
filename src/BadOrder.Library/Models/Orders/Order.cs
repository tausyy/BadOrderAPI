using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BadOrder.Library.Models.Orders
{
    public record Order : ModelBase
    {
        public string OwnerEmail { get; init; }
        public DateTimeOffset DateCreated { get; init; }
        public IEnumerable<OrderItem> OrderList { get; init; } 
    }
}
