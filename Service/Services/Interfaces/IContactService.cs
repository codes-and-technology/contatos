using RegionalContacts.Core.Dto;
using RegionalContacts.Core.Entity;

namespace RegionalContacts.Service.Services.Interfaces;

public interface IContactService
{
    Task<Result<Contact>> AddAsync(ContactDto dto);

    Task<IList<ContactDto>> FindAsync(short? regionId);
}
