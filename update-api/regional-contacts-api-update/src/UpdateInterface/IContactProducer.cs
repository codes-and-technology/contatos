using UpdateEntitys;

namespace UpdateInterface;

public interface IContactProducer
{
    Task SendMessage(ContactEntity entity);
}
