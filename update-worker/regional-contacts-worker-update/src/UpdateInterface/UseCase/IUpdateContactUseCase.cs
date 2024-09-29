using UpdateEntitys;
using Presenters;

namespace UpdateInterface.UseCase
{
    public interface IUpdateContactUseCase
    {
        UpdateResult<ContactEntity> Update(ContactEntity entity);
    }
}
