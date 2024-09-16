using ConsultingEntitys;
using ConsultingInterface.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DataBase.SqlServer;

public class ContactRepository(ApplicationDbContext context) : Repository<ContactEntity>(context), IContactRepository
{
    public async Task<IEnumerable<ContactEntity>> GetAllAsync()
    {
        return await Task.Run(() => context.Contacts.Include(i => i.PhoneRegion).ToList());
    }
}
