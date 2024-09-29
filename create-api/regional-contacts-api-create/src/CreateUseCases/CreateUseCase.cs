using CreateEntitys;
using Presenters;

namespace CreateUseCases;

public class CreateUseCase
{
    private readonly ContactDto _contactDto;
    private readonly IEnumerable<ContactConsultingDto> _contactConsultingDto;

    public CreateUseCase(ContactDto contactDto, IEnumerable<ContactConsultingDto> contactConsultingDto)
    {
        _contactDto = contactDto;        
        _contactConsultingDto = contactConsultingDto;
    }

    public ResultDto<ContactEntity> CreateContact()
    {
        var result = new ResultDto<ContactEntity>();
        result.Valid(_contactDto);

        if (_contactConsultingDto.Any(a => a.PhoneNumber == _contactDto.PhoneNumber && a.RegionNumber.Equals(_contactDto.RegionNumber)))
        {
            result.Errors.Add("Número de telefone já existe");        
            return result;
        }

        if (result.Errors.Count > 0)
            return result;

        return CreateContactEntity();
    }

    private ResultDto<ContactEntity> CreateContactEntity()
    {
        var result = new ResultDto<ContactEntity>();
        var contact = new ContactEntity
        {
            Id = Guid.NewGuid(),
            Name = _contactDto.Name,
            Email = _contactDto.Email,
            PhoneNumber = _contactDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            PhoneRegion = new PhoneRegionEntity { RegionNumber = _contactDto.RegionNumber }
        };

        result.Data = contact;
        return result;
    }
}
