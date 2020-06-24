using System;

namespace Standard.Framework.Seedworks.Events.Concrete
{
    public class IntegrationEvent
    {
        public IntegrationEvent(Guid? eventId = null) 
        {
            EventId = eventId.HasValue ? eventId.Value : Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        public Guid EventId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
