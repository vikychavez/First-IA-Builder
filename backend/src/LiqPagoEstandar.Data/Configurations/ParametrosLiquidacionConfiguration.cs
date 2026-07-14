using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class ParametrosLiquidacionConfiguration : IEntityTypeConfiguration<ParametrosLiquidacionEntity>
{
    // RF-28/R-01: valores regulatorios vigentes al momento de construir la aplicación.
    public const int IdUnico = 1;

    public void Configure(EntityTypeBuilder<ParametrosLiquidacionEntity> builder)
    {
        builder.ToTable("ParametrosLiquidacion");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PorcentajeAntiguedad).HasColumnType("decimal(9,4)");
        builder.Property(p => p.PorcentajeZonaDesfavorable).HasColumnType("decimal(9,4)");
        builder.Property(p => p.MultiplicadorHorasExtras).HasColumnType("decimal(9,4)");
        builder.Property(p => p.MultiplicadorFeriados).HasColumnType("decimal(9,4)");

        builder.HasData(new ParametrosLiquidacionEntity
        {
            Id = IdUnico,
            PorcentajeAntiguedad = 0.01m,
            PorcentajeZonaDesfavorable = 0.31m,
            MultiplicadorHorasExtras = 1.50m,
            MultiplicadorFeriados = 2m
        });
    }
}
