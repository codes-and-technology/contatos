using CreateDto;
using CreateEntitys;
using CreateInterface;
using CreateUseCases;

namespace CreateController;

public class CreateContactController : IController
{
    public readonly IContactProducer _contactProducer;

    public CreateContactController(IContactProducer contactProducer)
    {
        _contactProducer = contactProducer;
    }

    public async Task<ResultDto<ContactEntity>> CreateAsync(ContactDto contactDto)
    {
        var resultDto = new ResultDto<ContactEntity>();
        var createContactUseCase = new CreateUseCase(contactDto, resultDto);

        var result = createContactUseCase.CreateContact();

        if (result.Success)
            await _contactProducer.SendMessage(result.Data);

        return result;
    }
}
