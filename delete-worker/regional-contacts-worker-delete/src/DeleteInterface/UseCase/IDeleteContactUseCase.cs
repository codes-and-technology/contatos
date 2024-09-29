using DeleteEntitys;
using Presenters;

namespace DeleteInterface.UseCase
{
    public interface IDeleteContactUseCase
    {
        DeleteResult<ContactEntity> Delete(ContactEntity entity);
    }
}
