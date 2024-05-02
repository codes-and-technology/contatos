using RegionalContacts.Core.Dto;
using RegionalContacts.Core.Entity;

namespace RegionalContacts.Service.Services.Interfaces;

public interface IContactService
{
    Task<Result<Contact>> AddAsync(ContactDto dto);

    Task<IList<ContactDto>> FindAsync(short? regionId);
    Task<Result<Contact>> UpdateAsync(Guid id, ContactDto dto);
    Task<Result<Contact>> DeleteAsync(Guid id);
}
