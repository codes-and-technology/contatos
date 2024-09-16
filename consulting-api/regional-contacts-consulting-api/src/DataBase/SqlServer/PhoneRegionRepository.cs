using ConsultingEntitys;
using ConsultingInterface.DataBase;

namespace DataBase.SqlServer;

public class PhoneRegionRepository(ApplicationDbContext context) : Repository<PhoneRegionEntity>(context), IPhoneRegionRepository
{}
