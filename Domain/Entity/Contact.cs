namespace RegionalContacts.Core.Entity;

public class Contact : EntityBase
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public PhoneRegion PhoneRegion { get; set; }
}
