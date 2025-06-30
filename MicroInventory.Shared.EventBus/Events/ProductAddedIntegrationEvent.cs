using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroInventory.Shared.EventBus.Abstractions;

namespace MicroInventory.Shared.EventBus.Events
{
    public class ProductAddedIntegrationEvent : IntegrationEvent
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
    }
}
