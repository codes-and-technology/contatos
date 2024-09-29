using UpdateEntitys;

namespace Presenters;

public class UpdateContactEntity
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public PhoneRegionEntity PhoneRegion { get; set; }
    public Guid Id { get; set; }
}
