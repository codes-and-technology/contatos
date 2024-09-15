using UpdateEntitys.Base;

namespace UpdateEntitys;

public class PhoneRegionEntity : EntityBase
{
    public short RegionNumber { get; set; }
    public ICollection<UpdateContactEntity> Contacts { get; set; } = [];
}
