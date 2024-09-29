using CreateEntitys;
using CreateInterface.Controllers;
using CreateInterface.Gateway.Queue;
using Presenters;

namespace QueueGateways
{
    public class CreateContactGateway(ICreateContactController createContactController) : ICreateContactGateway
    {
        private readonly ICreateContactController _createContactController = createContactController;

        public async Task<CreateResult<ContactEntity>> CreateAsync(ContactEntity entity)
        {
            return await _createContactController.CreateAsync(entity);
        }
    }
}
