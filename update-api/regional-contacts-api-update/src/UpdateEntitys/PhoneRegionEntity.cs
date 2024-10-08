﻿using UpdateEntitys.Base;

namespace UpdateEntitys;

public class PhoneRegionEntity : EntityBase
{
    public short RegionNumber { get; set; }
    public ICollection<ContactEntity> Contacts { get; set; } = [];
}
