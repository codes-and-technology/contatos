using DeleteEntitys;
using Presenters;

namespace DeleteUseCases;

public class DeleteUseCase
{
    private readonly Guid _id;
    private readonly IEnumerable<ContactConsultingDto> _contactList;
    public DeleteUseCase(Guid id, IEnumerable<ContactConsultingDto> contactList)
    {
        _id = id;
        _contactList = contactList;
    }

    public ResultDto<ContactEntity> DeleteContact()
    {
        var result = new ResultDto<ContactEntity>();

        if (Guid.Empty == _id)
        {
            result.Errors.Add("Id não informado");
            return result;
        }

        if (!_contactList.Any(a => a.Id.Equals(_id.ToString(), StringComparison.InvariantCulture)))
        {
            result.Errors.Add("Contato não encontrado");
            return result;
        }

        if (result.Errors.Count > 0)
            return result;

        return DeleteContactEntity();
    }   

    public ResultDto<ContactEntity> DeleteContactEntity()
    {
        var contact = _contactList.FirstOrDefault(a => a.Id.Equals(_id.ToString(), StringComparison.InvariantCulture));
        return new ResultDto<ContactEntity>()
        {
            Data = new ContactEntity
            {
                Id = _id,
                Email = contact.Email,
                Name = contact.Name,
                PhoneNumber = contact.PhoneNumber,
                PhoneRegion = new PhoneRegionEntity { RegionNumber = short.Parse(contact.RegionNumber.ToString()) }
            }
        };
    }
}
