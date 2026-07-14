using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class PersonalConfiguration : IEntityTypeConfiguration<PersonalEntity>
{
    public void Configure(EntityTypeBuilder<PersonalEntity> builder)
    {
        builder.ToTable("Personal");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Dni).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Direccion).IsRequired().HasMaxLength(300);
        builder.Property(p => p.Telefono).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Provincia).IsRequired().HasMaxLength(100);
        builder.Property(p => p.HorasMensualesPactadas).HasColumnType("decimal(18,2)");

        // AC-15/AC-16: DNI único solo entre el personal activo; un DNI dado de baja
        // puede reutilizarse en un alta nueva, conservando el registro anterior como historial.
        builder.HasIndex(p => p.Dni)
            .IsUnique()
            .HasFilter("[Activo] = 1");

        builder.HasOne(p => p.Cliente)
            .WithMany()
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Categoria)
            .WithMany()
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
