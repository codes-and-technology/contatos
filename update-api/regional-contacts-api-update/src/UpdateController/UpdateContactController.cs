using Presenters;
using UpdateEntitys;
using UpdateInterface;
using UpdateUseCases;

namespace UpdateController;

public class UpdateContactController : IController
{
    private readonly IContactQueueGateway _contactQueuGateway;

    public UpdateContactController(IContactQueueGateway contactQueuGateway)
    {
        _contactQueuGateway = contactQueuGateway;
    }

    public async Task<ResultDto<UpdateContactEntity>> UpdateAsync(Guid id, ContactDto contactDto)
    {        
        var updateContactUseCase = new UpdateUseCase(contactDto, id);
       
        var result = updateContactUseCase.UpdateContact();

        if (result.Success)
            await _contactQueuGateway.SendMessage(result.Data);

        return result;
    }
}
