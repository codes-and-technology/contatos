using RegionalContacts.Core.Entity;

namespace RegionalContacts.Core.Repositories.Interfaces;

public interface IContactRepository
{
    Task AddAsync(Contact entity);
    Task<Contact> FindByPhoneNumberAsync(string number, short regionId);

    Task<IList<Contact>> FindAllAsync();
}
