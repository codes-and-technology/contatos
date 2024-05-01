using Infrastructure.Repository;
using RegionalContacts.Core.Entity;
using RegionalContacts.Core.Repositories.Interfaces;
using RegionalContacts.Infrastructure.Repository;

namespace RegionalContacts.Infrastructure.Repositories;

public class ContactRepository(ApplicationDbContext context) : Repository<Contact>(context), IContactRepository {}
