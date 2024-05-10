namespace RegionalContacts.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IContactRepository Contacts { get; }
    IPhoneRegionRepository PhoneRegions { get; }
    Task<int> CommitAsync();
}
