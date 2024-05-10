using Microsoft.EntityFrameworkCore;
using RegionalContacts.Domain.Entity;
using RegionalContacts.Domain.Interfaces.Repositories;

namespace RegionalContacts.Infrastructure.Repositories.SqlServer;

public class ContactRepository(ApplicationDbContext context) : Repository<Contact>(context), IContactRepository
{
    public override async Task<IList<Contact>> FindAllAsync()
    {
        return await Task.Run(() => context.Contacts.Include(i => i.PhoneRegion).ToList());
    }

    public override async Task<Contact> FindByIdAsync(Guid id)
    {
        return await context.Contacts.Include(i => i.PhoneRegion).Where(f => f.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task<Contact> FindByPhoneNumberAsync(string number, short regionId)
    {
        return await context.Contacts.Where(f => f.PhoneNumber == number && f.PhoneRegion.RegionNumber == regionId).FirstOrDefaultAsync();
    }
}
