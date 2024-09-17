using Presenters;
using DeleteEntitys;
using DeleteInterface.Controllers;
using DeleteInterface.Gateway.Cache;
using DeleteInterface.Gateway.DB;
using DeleteInterface.UseCase;

namespace DeleteController
{
    public class DeleteContactController(IContactDBGateway contactDBGateway,
                                         IDeleteContactUseCase deleteContactUseCase,
                                         ICacheGateway<ContactEntity> cache) : IDeleteContactController
    {
        private readonly IContactDBGateway _contactDBGateway = contactDBGateway;
        private readonly IDeleteContactUseCase _deleteContactUseCase = deleteContactUseCase;
        private readonly ICacheGateway<ContactEntity> _cache = cache;

        public async Task<DeleteResult<ContactEntity>> DeleteAsync(ContactEntity entity)
        {
            var result = _deleteContactUseCase.Delete(entity);

            if (result.Errors.Count > 0)
                return result;

            await _contactDBGateway.DeleteAsync(entity);
            await _contactDBGateway.CommitAsync();

            await _cache.ClearCacheAsync("Contacts");

            return result;
        }
    }
}
