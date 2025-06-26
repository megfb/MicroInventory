using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroInventory.Shared.EventBus.Abstractions
{

    //integrationevent ler için base class. 
    public abstract class IntegrationEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; }  = DateTime.UtcNow;


    }
}
