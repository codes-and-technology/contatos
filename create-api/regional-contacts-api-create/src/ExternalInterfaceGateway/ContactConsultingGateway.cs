using Presenters;
using CreateInterface;
using External.Interfaces;

namespace ExternalInterfaceGateway;

public class ContactConsultingGateway : IContactConsultingGateway
{
    private readonly IContactExternal _contactConsultingApi;

    public ContactConsultingGateway(IContactExternal contactConsultingApi)
    {
        _contactConsultingApi = contactConsultingApi;
    }

    public async Task<IEnumerable<ContactConsultingDto>> Get(int regionId)
    {
        var result = await _contactConsultingApi.Get(regionId);

        if (!result.IsSuccessStatusCode)
            throw new Exception("Falha ao tentar consultar contato");

        return result.Content;
    }
}
