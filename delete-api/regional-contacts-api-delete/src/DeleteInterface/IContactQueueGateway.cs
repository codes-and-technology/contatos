using DeleteEntitys;

namespace DeleteInterface;

public interface IContactQueueGateway
{
    Task SendMessage(ContactEntity entity);
}
