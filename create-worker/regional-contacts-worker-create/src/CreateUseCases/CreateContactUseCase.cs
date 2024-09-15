using CreateEntitys;
using CreateInterface.UseCase;
using Presenters;

namespace CreateUseCases.UseCase
{
    public class CreateContactUseCase : ICreateContactUseCase
    {
        public CreateResult<ContactEntity> Create(ContactEntity entity)
        {
            var result = new CreateResult<ContactEntity>(entity);
            result.Valid(entity);
            return result;
        }
    }
}
