using DeleteEntitys;

namespace DeleteInterface;

public interface IContactQueueGateway
{
    Task SendMessage(DeleteContactEntity entity);
}
