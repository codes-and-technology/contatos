using Presenters;
using UpdateEntitys;
using UpdateInterface.Controllers;
using UpdateInterface.Gateway.Queue;

namespace QueueGateways
{
    public class UpdateContactGateway(IUpdateContactController updateContactController) : IUpdateContactGateway
    {
        private readonly IUpdateContactController _updateContactController = updateContactController;

        public async Task<UpdateResult<ContactEntity>> UpdateAsync(ContactEntity entity)
        {
            return await _updateContactController.UpdateAsync(entity);
        }
    }
}
