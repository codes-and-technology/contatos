using CreateEntitys;
using Presenters;

namespace CreateInterface.UseCase
{
    public interface ICreateContactUseCase
    {
        CreateResult<ContactEntity> Create(ContactEntity entity);
    }
}
