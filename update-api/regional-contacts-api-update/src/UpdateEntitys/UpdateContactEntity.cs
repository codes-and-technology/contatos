using UpdateEntitys.Base;

namespace UpdateEntitys;

public class UpdateContactEntity : EntityBase
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public PhoneRegionEntity PhoneRegion { get; set; }
}
