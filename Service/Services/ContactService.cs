using RegionalContacts.Core.Dto;
using RegionalContacts.Core.Dto.Contato;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Helpers.Validations;
using RegionalContacts.Core.Repositories.Interfaces;
using RegionalContacts.Service.Services.Interfaces;

namespace RegionalContacts.Service.Services;

public partial class ContactService(IUnitOfWork unitOfWork) : IContactService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IList<ContactDto>> FindAsync(short? regionId)
    {
        List<ContactDto> list = [];

        var result = await _unitOfWork.Contacts.FindAllAsync();

        if (regionId.HasValue)
            result = result.Where(f => f.PhoneRegion.RegionNumber == regionId.Value).ToList();

        foreach (var item in result)
        {
            list.Add(new ContactDto
            {
                Id = item.Id.ToString(),
                Email = item.Email,
                Name = item.Name,
                PhoneNumber = item.PhoneNumber,
                RegionNumber = item.PhoneRegion.RegionNumber
            });
        }

        return list;
    }

    public async Task<Result<Contact>> CreateAsync(ContactCreateDto dto)
    {
        var result = new Result<Contact>();
        result.Valid(dto);

        if (!result.Success)
            return result;

        var contact = await _unitOfWork.Contacts.FindByPhoneNumberAsync(dto.PhoneNumber, dto.RegionNumber);

        if (contact is not null)
        {
            result.Errors.Add("Contato já existe");
            return result;
        }

        if (result.Errors.Count > 0)
            return result;

        PhoneRegion phoneRegion = await ValidatePhoneRegionAsync(dto.RegionNumber);

        contact = new()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CreatedDate = DateTime.Now,            
            PhoneRegion = phoneRegion
        };

        await _unitOfWork.Contacts.AddAsync(contact);

        await _unitOfWork.CommitAsync();

        result.Data = contact;

        return result;
    }

    public async Task<Result<Contact>> UpdateAsync(Guid id, ContactUpdateDto dto)
    {
        var result = new Result<Contact>();
        result.Valid(dto);

        if (!result.Success)
            return result;

        var contact = await _unitOfWork.Contacts.FindByIdAsync(id);
        if (contact is null)
        {
            result.Errors.Add("Contato não encontrado");
            return result;
        }
        
        var contactWithNumberAndRegion = await _unitOfWork.Contacts.FindByPhoneNumberAsync(dto.PhoneNumber, dto.RegionNumber);
        if (contactWithNumberAndRegion is not null && contactWithNumberAndRegion.Id != id)
        {
            result.Errors.Add("Já existe um contato com esse número de telefone e ddd");
            return result;
        }

        contact.Name = dto.Name;
        contact.Email = dto.Email;
        contact.PhoneNumber = dto.PhoneNumber;
        contact.PhoneRegion = await ValidatePhoneRegionAsync(dto.RegionNumber);

        await _unitOfWork.CommitAsync();

        result.Data = contact;
        return result;
    }

    public async Task<Result<Contact>> DeleteAsync(Guid id)
    {
        var result = new Result<Contact>();
        var contact = await _unitOfWork.Contacts.FindByIdAsync(id);

        if (contact is null)
        {
            result.Errors.Add("Contato não encontrado");
            return result;
        }

        await _unitOfWork.Contacts.DeleteAsync(contact);
        await _unitOfWork.CommitAsync();

        return result;
    }

    private async Task<PhoneRegion> ValidatePhoneRegionAsync(short regionNumber)
    {
        var phoneRegion = await _unitOfWork.PhoneRegions.GetByRegionNumberAsync(regionNumber);

        phoneRegion ??= new PhoneRegion { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = regionNumber };

        return phoneRegion;
    }
}
