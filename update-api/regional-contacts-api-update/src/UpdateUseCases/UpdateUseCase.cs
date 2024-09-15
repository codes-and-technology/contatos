using UpdateEntitys;
using Presenters;

namespace UpdateUseCases;

public class UpdateUseCase
{
    private readonly ContactDto _contactDto;
    private readonly Guid _id;
    public UpdateUseCase(ContactDto contactDto, Guid id)
    {
        _contactDto = contactDto;
        _id = id;   
    }

    public ResultDto<UpdateContactEntity> UpdateContact()
    {
        var result = new ResultDto<UpdateContactEntity>();
        result.Valid(_contactDto);

        if (Guid.Empty == _id)
        {
            result.Errors.Add("Id não informado");
            return result;
        }

        if (result.Errors.Count > 0)
            return result;

        return UpdateContactEntity();
    }

    private ResultDto<UpdateContactEntity> UpdateContactEntity()
    {
        var result = new ResultDto<UpdateContactEntity>();
        var contact = new UpdateContactEntity
        {
            Id = _id,
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
