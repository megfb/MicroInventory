﻿using MicroInventory.Shared.Common.Domain;

namespace MicroInventory.Category.Api.Domain.Entities
{
    public class Categories : Entity<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
