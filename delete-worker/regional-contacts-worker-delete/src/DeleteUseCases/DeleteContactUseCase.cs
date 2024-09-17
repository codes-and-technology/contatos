using DeleteEntitys;
using DeleteInterface.UseCase;
using Presenters;

namespace DeleteUseCases.UseCase
{
    public class DeleteContactUseCase : IDeleteContactUseCase
    {
        public DeleteResult<ContactEntity> Delete(ContactEntity entity)
        {
            var result = new DeleteResult<ContactEntity>(entity);

            if(entity.Id == Guid.Empty)
                result.Errors.Add("O contato deve ser informado");

            return result;
        }
    }
}
