﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Items.Dtos
{
    public record NewItemRequest
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Must be less than {1} characters")]
        public string ProductName { get; init; }

        public int? ProductNumber { get; init; }

        [Required]
        public UnitTypes UnitType { get; init; } = UnitTypes.Any;
    }
}