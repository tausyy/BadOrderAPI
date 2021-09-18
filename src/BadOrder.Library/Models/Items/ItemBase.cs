using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Items
{
    public record ItemBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string ProductName { get; init; }
        public bool IsNeeded { get; init; }
    }
}
