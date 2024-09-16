﻿using UpdateEntitys;
using UpdateInterface;

namespace QueueGateway;

public class ContactQueueGateway : IContactQueueGateway
{
    public readonly IContactProducer _contactProducer;

    public ContactQueueGateway(IContactProducer contactProducer)
    {
        _contactProducer = contactProducer;
    }

    public async Task SendMessage(UpdateContactEntity entity) => await _contactProducer.SendMessage(entity);    
}