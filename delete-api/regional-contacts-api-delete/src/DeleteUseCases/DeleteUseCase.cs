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

    public ResultDto<DeleteContactEntity> DeleteContact()
    {
        var result = new ResultDto<DeleteContactEntity>();

        if (Guid.Empty == _id)
        {
            result.Errors.Add("Id não informado");
            return result;
        }

        if (!_contactList.Any(a => a.Id.Equals(_id.ToString())))
        {
            result.Errors.Add("Contato não encontrado");
            return result;
        }

        if (result.Errors.Count > 0)
            return result;

        return DeleteContactEntity();
    }   

    public ResultDto<DeleteContactEntity> DeleteContactEntity()
    {
        return new ResultDto<DeleteContactEntity>()
        {
            Data = new DeleteContactEntity
            {
                Id = _id,
            }
        };
    }
}
