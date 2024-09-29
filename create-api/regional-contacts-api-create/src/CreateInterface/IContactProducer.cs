using CreateEntitys;

namespace CreateInterface;

public interface IContactProducer
{
    Task SendMessage(ContactEntity entity);
}
