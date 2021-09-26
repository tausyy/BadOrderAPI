using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Orders
{
    public record OrderItem
    {
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ItemId { get; init; }

        [MaxLength(50, ErrorMessage = "Must be less than {1} characters")]
        [MinLength(0, ErrorMessage = "Cannot be less than 0")]
        public int Amount { get; init; } = 0;
    }
}
