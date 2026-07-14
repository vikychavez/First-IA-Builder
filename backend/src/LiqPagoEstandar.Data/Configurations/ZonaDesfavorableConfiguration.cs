using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class ZonaDesfavorableConfiguration : IEntityTypeConfiguration<ZonaDesfavorableEntity>
{
    public void Configure(EntityTypeBuilder<ZonaDesfavorableEntity> builder)
    {
        builder.ToTable("ZonasDesfavorables");

        builder.HasKey(z => z.Id);

        builder.Property(z => z.Provincia)
            .IsRequired()
            .HasMaxLength(100);

        // AC-21: no permite duplicar una provincia ya activa (pero sí reingresar una dada de baja).
        builder.HasIndex(z => z.Provincia)
            .IsUnique()
            .HasFilter("[Activo] = 1");
    }
}
