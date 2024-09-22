namespace DeleteEntitys;

public class PhoneRegionEntity
{
    public short RegionNumber { get; set; }
    public ICollection<ContactEntity> Contacts { get; set; } = [];
}
