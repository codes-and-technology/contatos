using CreateDto;
using CreateEntitys;
using CreateUseCases.Helpers;

namespace CreateUseCases;

public class CreateUseCase
{
    private readonly ContactDto _contactDto;
    private readonly ResultDto<ContactEntity> _result;

    public CreateUseCase(ContactDto contactDto, ResultDto<ContactEntity> result)
    {
        _contactDto = contactDto;
        _result = result;
    }

    public ResultDto<ContactEntity> CreateContact()
    {
        var result = new ResultDto<ContactEntity>();
        result.Valid(_contactDto);

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
