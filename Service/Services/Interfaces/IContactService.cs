using RegionalContacts.Core.Dto;
using RegionalContacts.Core.Dto.Contato;
using RegionalContacts.Core.Entity;

namespace RegionalContacts.Service.Services.Interfaces;

public interface IContactService
{
    Task<IList<ContactDto>> FindAsync(short? regionId);
    Task<Result<Contact>> CreateAsync(ContactCreateDto dto);    
    Task<Result<Contact>> UpdateAsync(Guid id, ContactUpdateDto dto);
    Task<Result<Contact>> DeleteAsync(Guid id);
}
