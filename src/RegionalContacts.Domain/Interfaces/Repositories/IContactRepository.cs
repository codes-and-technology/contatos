using RegionalContacts.Domain.Entity;

namespace RegionalContacts.Domain.Interfaces.Repositories;

public interface IContactRepository : IRepository<Contact>
{
    Task<Contact> FindByPhoneNumberAsync(string number, short regionId);
}
