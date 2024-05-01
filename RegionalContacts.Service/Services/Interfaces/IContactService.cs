using RegionalContacts.Core.Dto;

namespace RegionalContacts.Service.Services.Interfaces;

public interface IContactService
{
    Task AddAsync(ContactDto dto);
}
