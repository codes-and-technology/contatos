using Presenters;
using DeleteEntitys;
using DeleteInterface;
using DeleteUseCases;

namespace Controller;

public class DeleteContactController : IController
{
    private readonly IContactQueueGateway _contactQueuGateway;
    private readonly IContactConsultingGateway _contactConsultingGateway;

    public DeleteContactController(IContactQueueGateway contactQueuGateway, IContactConsultingGateway contactConsultingGateway)
    {
        _contactQueuGateway = contactQueuGateway;
        _contactConsultingGateway = contactConsultingGateway;
    }

    public async Task<ResultDto<DeleteContactEntity>> DeleteAsync(Guid id)
    {
        var list = await _contactConsultingGateway.Get();

        var updateContactUseCase = new DeleteUseCase(id, list);
       
        var result = updateContactUseCase.DeleteContact();

        if (result.Success)
            await _contactQueuGateway.SendMessage(result.Data);

        return result;
    }
}
