namespace DeleteEntitys;

public class ContactEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public PhoneRegionEntity PhoneRegion { get; set; }
}
