using UpdateEntitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.SqlServer.Configurations;

public class PhoneRegionConfiguration : IEntityTypeConfiguration<PhoneRegionEntity>
{
    public void Configure(EntityTypeBuilder<PhoneRegionEntity> builder)
    {
        builder.ToTable("TelefoneRegiao");
        builder.HasKey(k => k.Id);
        builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
        builder.Property(p => p.CreatedDate).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.RegionNumber).HasColumnName("CodigoArea").IsRequired();
    }
}
