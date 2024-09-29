using UpdateEntitys;
using UpdateInterface.DataBase;

namespace DataBase.SqlServer;

public class ContactRepository(ApplicationDbContext context) : Repository<ContactEntity>(context), IContactRepository
{
}
