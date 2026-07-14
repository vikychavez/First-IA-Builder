using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
{
    public void Configure(EntityTypeBuilder<ClienteEntity> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Dni).IsRequired().HasMaxLength(8);
        builder.Property(c => c.Cuit).IsRequired().HasMaxLength(11);
        builder.Property(c => c.Sexo).IsRequired().HasMaxLength(1);
        builder.Property(c => c.Apellido).IsRequired().HasMaxLength(100);

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.Telefono).HasMaxLength(50);
        builder.Property(c => c.Direccion).HasMaxLength(300);

        // AC-43: DNI único solo entre los clientes activos (mismo patrón que Personal).
        builder.HasIndex(c => c.Dni)
            .IsUnique()
            .HasFilter("[Activo] = 1");
    }
}
