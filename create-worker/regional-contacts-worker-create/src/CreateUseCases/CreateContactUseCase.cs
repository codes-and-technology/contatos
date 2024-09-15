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

            if(entity.PhoneRegion?.Id == Guid.Empty)
                result.Errors.Add("O DDD deve ser informado");

            return result;
        }
    }
}
