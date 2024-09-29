﻿using DeleteEntitys;
using DeleteInterface.DataBase;
using DeleteInterface.Gateway.DB;

namespace DBGateways
{
    public class PhoneRegionDBGateway(IUnitOfWork unitOfWork) : BaseDB(unitOfWork), IPhoneRegionDBGateway
    {
        public async Task AddAsync(PhoneRegionEntity entity)
        {
            await Uow.PhoneRegions.AddAsync(entity);
        }

        public async Task<PhoneRegionEntity> GetByRegionNumberAsync(short regionNumber)
        {
            return await Uow.PhoneRegions.GetByRegionNumberAsync(regionNumber);
        }
    }
}
