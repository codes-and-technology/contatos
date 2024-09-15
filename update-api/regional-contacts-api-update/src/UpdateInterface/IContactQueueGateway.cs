using UpdateEntitys;

namespace UpdateInterface;

public interface IContactQueueGateway
{
    Task SendMessage(UpdateContactEntity entity);
}
