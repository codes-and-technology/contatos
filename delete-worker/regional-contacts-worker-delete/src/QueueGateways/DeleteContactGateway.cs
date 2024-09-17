using Presenters;
using DeleteEntitys;
using DeleteInterface.Controllers;
using DeleteInterface.Gateway.Queue;

namespace QueueGateways
{
    public class DeleteContactGateway(IDeleteContactController deleteContactController) : IDeleteContactGateway
    {
        private readonly IDeleteContactController _deleteContactController = deleteContactController;

        public async Task<DeleteResult<ContactEntity>> DeleteAsync(ContactEntity entity)
        {
            return await _deleteContactController.DeleteAsync(entity);
        }
    }
}
