﻿using UpdateEntitys;

namespace UpdateInterface.DataBase;

public interface IPhoneRegionRepository : IRepository<PhoneRegionEntity>
{
    Task<PhoneRegionEntity> GetByRegionNumberAsync(short regionNumber);
}
