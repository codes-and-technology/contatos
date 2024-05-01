using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegionalContacts.Core.Entity;

namespace RegionalContacts.Infrastructure.Repository.Configurations;

public class PhoneRegionConfiguration : IEntityTypeConfiguration<PhoneRegion>
{
    public void Configure(EntityTypeBuilder<PhoneRegion> builder)
    {
        builder.ToTable("TelefoneRegiao");
        builder.HasKey(k => k.Id);        
        builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
        builder.Property(p => p.CreatedDate).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.RegionNumber).HasColumnName("CodigoArea").IsRequired();
    }
}
