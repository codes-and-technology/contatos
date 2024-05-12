using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegionalContacts.Domain.Entity;

namespace RegionalContacts.Infrastructure.Repositories.SqlServer.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contato");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.CreatedDate).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
        builder.Property(p => p.Name).HasColumnName("Nome").HasColumnType("VARCHAR(250)").IsRequired();
        builder.Property(p => p.Email).HasColumnName("Email").HasColumnType("VARCHAR(250)").IsRequired();
        builder.Property(p => p.PhoneNumber).HasColumnName("NumeroTelefone").HasColumnType("VARCHAR(9)").IsRequired();

        builder.HasOne(o => o.PhoneRegion)
            .WithMany(m => m.Contacts)
            .HasPrincipalKey(p => p.Id)
            .HasConstraintName("IdArea");
    }
}
