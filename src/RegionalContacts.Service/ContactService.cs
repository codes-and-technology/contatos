using RegionalContacts.Domain.Dto;
using RegionalContacts.Domain.Dto.Contato;
using RegionalContacts.Domain.Entity;
using RegionalContacts.Domain.Helpers.Validations;
using RegionalContacts.Domain.Interfaces.Repositories;
using RegionalContacts.Infrastructure.Repositories.Redis;
using RegionalContacts.Service.Services.Interfaces;

namespace RegionalContacts.Service
{
    public partial class ContactService(IUnitOfWork unitOfWork, IRedisCache<ContactDto> redis) : IContactService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRedisCache<ContactDto> _redis = redis;

        public async Task<IList<ContactDto>> FindAsync(short? regionId)
        {
            List<ContactDto> list = [];

            var contactsCache = await _redis.GetCacheAsync("Contacts");

            if (contactsCache.Any())
            {
                if (regionId.HasValue)
                    return contactsCache.Where(f => !regionId.HasValue || f.RegionNumber == regionId.Value).ToList();

                return contactsCache;
            }

            var result = await _unitOfWork.Contacts.FindAllAsync();

            await _redis.SaveCacheAsync("Contacts", result.Select(f => new ContactDto
            {
                Id = f.Id.ToString(),
                Email = f.Email,
                Name = f.Name,
                PhoneNumber = f.PhoneNumber,
                RegionNumber = f.PhoneRegion.RegionNumber
            }).ToList());

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

            PhoneRegion phoneRegion = await GetOrCreatePhoneRegionAsync(dto.RegionNumber);

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

            await _redis.ClearCacheAsync("Contacts");

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
            contact.PhoneRegion = await GetOrCreatePhoneRegionAsync(dto.RegionNumber);     

            await _unitOfWork.CommitAsync();

            var contactsCache = await _redis.GetCacheAsync("Contacts");

            contactsCache.Where(f => f.Id == id.ToString()).ToList().ForEach(f =>
            {
                f.Email = contact.Email;
                f.Name = contact.Name;
                f.PhoneNumber = contact.PhoneNumber;
                f.RegionNumber = contact.PhoneRegion.RegionNumber;
            });

            await _redis.ClearCacheAsync("Contacts");            

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

            await _redis.ClearCacheAsync("Contacts");

            return result;
        }

        private async Task<PhoneRegion> GetOrCreatePhoneRegionAsync(short regionNumber)
        {
            var phoneRegion = await _unitOfWork.PhoneRegions.GetByRegionNumberAsync(regionNumber);

            if (phoneRegion is null)
            {
                phoneRegion = new PhoneRegion { CreatedDate = DateTime.Now, Id = Guid.NewGuid(), RegionNumber = regionNumber };
                await _unitOfWork.PhoneRegions.AddAsync(phoneRegion);
            }

            return phoneRegion;
        }
    }
}