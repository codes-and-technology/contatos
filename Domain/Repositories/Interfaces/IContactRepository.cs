using RegionalContacts.Core.Entity;

namespace RegionalContacts.Core.Repositories.Interfaces;

public interface IContactRepository
{
    Task AddAsync(Contact entity);
}
