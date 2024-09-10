using CreateDto;
using CreateEntitys;
using CreateDto.Helpers;

namespace CreateUseCases;

public class CreateUseCase
{
    private readonly ContactDto _contactDto;
    private readonly ResultDto<ContactEntity> _result;
    private readonly IEnumerable<ContactConsultingDto> _contactConsultingDto;

    public CreateUseCase(ContactDto contactDto, ResultDto<ContactEntity> result, IEnumerable<ContactConsultingDto> contactConsultingDto)
    {
        _contactDto = contactDto;
        _result = result;
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
            return _result;

        return CreateContactEntity();
    }

    private ResultDto<ContactEntity> CreateContactEntity()
    {
        var contact = new ContactEntity
        {
            Id = Guid.NewGuid(),
            Name = _contactDto.Name,
            Email = _contactDto.Email,
            PhoneNumber = _contactDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            PhoneRegion = new PhoneRegionEntity { RegionNumber = _contactDto.RegionNumber }
        };

        _result.Data = contact;
        return _result;
    }
}
