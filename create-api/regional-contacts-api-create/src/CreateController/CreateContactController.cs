using CreateDto;
using CreateEntitys;
using CreateInterface;
using CreateUseCases;

namespace CreateController;

public class CreateContactController : IController
{
    private readonly IContactQueueGateway _contactQueuGateway;
    private readonly IContactConsultingGateway _contactConsulting;

    public CreateContactController(IContactQueueGateway contactQueuGateway, IContactConsultingGateway contactConsulting)
    {
        _contactQueuGateway = contactQueuGateway;
        _contactConsulting = contactConsulting;
    }

    public async Task<ResultDto<ContactEntity>> CreateAsync(ContactDto contactDto)
    {
        var resultDto = new ResultDto<ContactEntity>();
        
        var contactList = await _contactConsulting.Get(contactDto.RegionNumber);

        var createContactUseCase = new CreateUseCase(contactDto, resultDto, contactList);
       
        var result = createContactUseCase.CreateContact();

        if (result.Success)
            await _contactQueuGateway.SendMessage(result.Data);

        return result;
    }
}
