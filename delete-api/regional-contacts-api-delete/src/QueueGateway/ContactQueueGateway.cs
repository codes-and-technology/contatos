using DeleteEntitys;
using DeleteInterface;

namespace QueueGateway;

public class ContactQueueGateway : IContactQueueGateway
{
    public readonly IContactProducer _contactProducer;

    public ContactQueueGateway(IContactProducer contactProducer)
    {
        _contactProducer = contactProducer;
    }

    public async Task SendMessage(ContactEntity entity) => await _contactProducer.SendMessage(entity);    
}
