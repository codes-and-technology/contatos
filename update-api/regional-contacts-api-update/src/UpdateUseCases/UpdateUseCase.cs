using UpdateEntitys;
using Presenters;

namespace UpdateUseCases;

public class UpdateUseCase
{
    private readonly ContactDto _contactDto;
    private readonly Guid _id;
    private readonly IEnumerable<ContactConsultingDto> _contactList;
    public UpdateUseCase(ContactDto contactDto, Guid id, IEnumerable<ContactConsultingDto> contactList)
    {
        _contactDto = contactDto;
        _id = id; 
        _contactList = contactList; 
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

        if (!_contactList.Any(a => a.Id.Equals(_id.ToString())))
        {
            result.Errors.Add("Contato não encontrado");
            return result;
        };        

        var contactWithNumberAndRegion = _contactList.Where(f =>
        f.RegionNumber == _contactDto.RegionNumber &&
        f.PhoneNumber == _contactDto.PhoneNumber
        );

        if (contactWithNumberAndRegion.Any(a => !a .Id.Equals(_id.ToString())))
        {
            result.Errors.Add("Já existe um contato com esse número de telefone e ddd");
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
