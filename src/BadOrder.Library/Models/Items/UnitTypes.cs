using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Items
{
    public enum UnitTypes
    {
        Any,
        Boxes,
        Packs,
        Pounds
    }
}
