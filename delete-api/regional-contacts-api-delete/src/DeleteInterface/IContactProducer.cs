using DeleteEntitys;

namespace DeleteInterface;

public interface IContactProducer
{
    Task SendMessage(DeleteContactEntity entity);
}
