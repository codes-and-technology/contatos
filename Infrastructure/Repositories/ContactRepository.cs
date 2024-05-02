using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Repositories.Interfaces;
using RegionalContacts.Infrastructure.Repository;

namespace RegionalContacts.Infrastructure.Repositories;

public class ContactRepository(ApplicationDbContext context) : Repository<Contact>(context), IContactRepository
{
    public async Task<IList<Contact>> FindAllAsync()
    {
        return  context.Contacts.Include(i => i.PhoneRegion).ToList();        
    }

    public async Task<Contact> FindByPhoneNumberAsync(string number, short regionId)
    {
        return await context.Contacts.Where(f => f.PhoneNumber == number && f.PhoneRegion.RegionNumber == regionId).FirstOrDefaultAsync();
    }

    public async Task<Contact> FindByIdAsync(Guid id)
    {
        return await context.Contacts.Include(i => i.PhoneRegion).Where(f => f.Id.Equals(id)).FirstOrDefaultAsync();
    }
}
