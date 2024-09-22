using Presenters;
using UpdateEntitys;
using UpdateInterface;
using UpdateUseCases;

namespace UpdateController;

public class UpdateContactController : IController
{
    private readonly IContactQueueGateway _contactQueuGateway;
    private readonly IContactConsultingGateway _contactConsultingGateway;

    public UpdateContactController(IContactQueueGateway contactQueuGateway, IContactConsultingGateway contactConsultingGateway)
    {
        _contactQueuGateway = contactQueuGateway;
        _contactConsultingGateway = contactConsultingGateway;
    }

    public async Task<ResultDto<ContactEntity>> UpdateAsync(Guid id, ContactDto contactDto)
    {
        var list = await _contactConsultingGateway.Get(contactDto.RegionNumber);

        var updateContactUseCase = new UpdateUseCase(contactDto, id, list);
       
        var result = updateContactUseCase.UpdateContact();

        if (result.Success)
            await _contactQueuGateway.SendMessage(result.Data);

        return result;
    }
}
