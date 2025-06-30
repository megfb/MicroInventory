using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroInventory.Shared.EventBus.Abstractions;

namespace MicroInventory.Shared.EventBus.Events
{
    public class PersonAddedIntegrationEvent : IntegrationEvent
    {
        public string PersonId { get; set; }
        public string Name { get; set; }
    }
}
