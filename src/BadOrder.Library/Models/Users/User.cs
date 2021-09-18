using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Users
{
    public record User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string Name { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Role { get; init; }
        public string Token { get; set; }
        public DateTimeOffset DateAdded { get; init; }
    }
}
