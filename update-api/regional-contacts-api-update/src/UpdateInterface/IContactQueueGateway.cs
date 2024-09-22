using UpdateEntitys;

namespace UpdateInterface;

public interface IContactQueueGateway
{
    Task SendMessage(ContactEntity entity);
}
