using CreateEntitys;

namespace CreateInterface;

public interface IContactQueueGateway
{
    Task SendMessage(ContactEntity entity);
}
