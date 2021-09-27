using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Items
{
    public record Item : ModelBase
    {   
        public string ProductName { get; init; }
        
        public int? ProductNumber { get; init; }

        public UnitTypes UnitType { get; init; } = UnitTypes.Any;
    }
}
