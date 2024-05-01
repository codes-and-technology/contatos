using RegionalContacts.Core.Dto;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Repositories.Interfaces;
using RegionalContacts.Service.Services.Interfaces;

namespace RegionalContacts.Service.Services;

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;

    public ContactService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(ContactDto dto)
    {        
        var phoneRegion = await _unitOfWork.PhoneRegions.GetByRegionNumberAsync(dto.RegionNumber);

        if (phoneRegion is null)
        {
            phoneRegion = new PhoneRegion { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = dto.RegionNumber };
        }

        Contact contact = new()
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid(),
            PhoneRegion = phoneRegion
        };

        await _unitOfWork.Contacts.AddAsync(contact);

        await _unitOfWork.CommitAsync();
    }
}
