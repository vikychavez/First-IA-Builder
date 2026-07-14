using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class ResumenPersonalDetalleConfiguration : IEntityTypeConfiguration<ResumenPersonalDetalleEntity>
{
    public void Configure(EntityTypeBuilder<ResumenPersonalDetalleEntity> builder)
    {
        builder.ToTable("ResumenPersonalDetalles");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.ClienteNombre).HasMaxLength(200);
        builder.Property(d => d.PersonalNombreCompleto).HasMaxLength(200);
        builder.Property(d => d.Dni).HasMaxLength(20);
        builder.Property(d => d.CategoriaNombre).HasMaxLength(100);
        builder.Property(d => d.PdfNombreArchivo).HasMaxLength(300);

        builder.Property(d => d.ValorHora).HasColumnType("decimal(18,2)");
        builder.Property(d => d.SueldoBasicoNormal).HasColumnType("decimal(18,2)");
        builder.Property(d => d.TotalHorasNormales).HasColumnType("decimal(18,2)");
        builder.Property(d => d.ItemHorasExtras).HasColumnType("decimal(18,2)");
        builder.Property(d => d.ItemAntiguedad).HasColumnType("decimal(18,2)");
        builder.Property(d => d.ItemFeriados).HasColumnType("decimal(18,2)");
        builder.Property(d => d.ItemZonaDesfavorable).HasColumnType("decimal(18,2)");
        builder.Property(d => d.TotalAPagar).HasColumnType("decimal(18,2)");
    }
}
