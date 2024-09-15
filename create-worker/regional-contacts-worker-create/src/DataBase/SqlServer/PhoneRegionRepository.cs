using CreateEntitys;
using CreateInterface.DataBase;

namespace DataBase.SqlServer;

public class PhoneRegionRepository(ApplicationDbContext context) : Repository<PhoneRegionEntity>(context), IPhoneRegionRepository
{
}
