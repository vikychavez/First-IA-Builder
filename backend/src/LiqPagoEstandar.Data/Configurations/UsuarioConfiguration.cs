using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiqPagoEstandar.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<UsuarioEntity>
{
    // Seed: usuario "admin" / contraseña "Admin123!" (documentada en backend/README.md, no en el código).
    private const string AdminPasswordHash = "$2a$11$IsOyMa2W/iCnfLZeXTsAauXuH8z5WTDD3gDP1D1GhzxfoVz0JPUaW";
    private static readonly DateTime SeedFechaCreacion = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Configure(EntityTypeBuilder<UsuarioEntity> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.NombreUsuario)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(u => u.NombreUsuario).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasData(new UsuarioEntity
        {
            Id = 1,
            NombreUsuario = "admin",
            PasswordHash = AdminPasswordHash,
            Activo = true,
            FechaCreacion = SeedFechaCreacion
        });
    }
}
