using RegionalContacts.Domain.Dto;
using RegionalContacts.Domain.Dto.Contato;
using RegionalContacts.Domain.Entity;

namespace RegionalContacts.Service.Services.Interfaces;

public interface IContactService
{
    Task<IList<ContactDto>> FindAsync(short? regionId);
    Task<Result<Contact>> CreateAsync(ContactCreateDto dto);    
    Task<Result<Contact>> UpdateAsync(Guid id, ContactUpdateDto dto);
    Task<Result<Contact>> DeleteAsync(Guid id);
}
