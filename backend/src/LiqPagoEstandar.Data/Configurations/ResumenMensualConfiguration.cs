using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class ResumenMensualConfiguration : IEntityTypeConfiguration<ResumenMensualEntity>
{
    public void Configure(EntityTypeBuilder<ResumenMensualEntity> builder)
    {
        builder.ToTable("ResumenesMensuales");

        builder.HasKey(r => r.Id);

        builder.HasIndex(r => new { r.Anio, r.Mes }).IsUnique();

        builder.HasMany(r => r.Detalles)
            .WithOne(d => d.ResumenMensual)
            .HasForeignKey(d => d.ResumenMensualId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
