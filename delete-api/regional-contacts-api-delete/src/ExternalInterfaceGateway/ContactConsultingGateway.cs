﻿using External.Interfaces;
using Presenters;
using DeleteInterface;

namespace ExternalInterfaceGateway;

public class ContactConsultingGateway : IContactConsultingGateway
{
    private readonly IContactExternal _contactConsultingApi;

    public ContactConsultingGateway(IContactExternal contactConsultingApi)
    {
        _contactConsultingApi = contactConsultingApi;
    }

    public async Task<IEnumerable<ContactConsultingDto>> Get()
    {
        var result = await _contactConsultingApi.Get();

        if (!result.IsSuccessStatusCode)
            throw new Exception("Falha ao tentar consultar contato");

        return result.Content;
    }
}
