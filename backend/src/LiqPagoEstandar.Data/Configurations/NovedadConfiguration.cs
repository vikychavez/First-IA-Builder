using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class NovedadConfiguration : IEntityTypeConfiguration<NovedadEntity>
{
    public void Configure(EntityTypeBuilder<NovedadEntity> builder)
    {
        builder.ToTable("Novedades");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.HorasNormales).HasColumnType("decimal(9,2)");
        builder.Property(n => n.HorasFeriado).HasColumnType("decimal(9,2)");
        builder.Property(n => n.HorasExtra).HasColumnType("decimal(9,2)");

        builder.HasIndex(n => new { n.PersonalId, n.Anio, n.Mes }).IsUnique();

        builder.HasOne(n => n.Personal)
            .WithMany()
            .HasForeignKey(n => n.PersonalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
