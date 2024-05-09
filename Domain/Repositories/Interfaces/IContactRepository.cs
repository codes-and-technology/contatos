using RegionalContacts.Core.Entity;

namespace RegionalContacts.Core.Repositories.Interfaces;

public interface IContactRepository
{
    Task<Contact> FindByPhoneNumberAsync(string number, short regionId);
    Task<IList<Contact>> FindAllAsync();
    Task<Contact?> FindByIdAsync(Guid id);
    Task AddAsync(Contact entity);
    Task DeleteAsync(Contact entity);
}
