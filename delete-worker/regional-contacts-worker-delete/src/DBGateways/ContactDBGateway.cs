using DeleteEntitys;
using DeleteInterface.DataBase;
using DeleteInterface.Gateway.DB;

namespace DBGateways
{
    public class ContactDBGateway(IUnitOfWork unitOfWork) : BaseDB(unitOfWork), IContactDBGateway
    {
        public async Task AddAsync(ContactEntity entity)
        {
            await Uow.Contacts.AddAsync(entity);
        }

        public async Task DeleteAsync(ContactEntity entity)
        {
            await Uow.Contacts.DeleteAsync(entity);
        }

        public async Task<ContactEntity> FindByIdAsync(Guid id)
        {
            return await Uow.Contacts.FindByIdAsync(id);
        }
    }
}