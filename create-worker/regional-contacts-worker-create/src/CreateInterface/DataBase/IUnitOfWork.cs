namespace CreateInterface.DataBase;

public interface IUnitOfWork : IDisposable
{
    IContactRepository Contacts { get; }
    IPhoneRegionRepository PhoneRegions { get; }
    Task<int> CommitAsync();
}

