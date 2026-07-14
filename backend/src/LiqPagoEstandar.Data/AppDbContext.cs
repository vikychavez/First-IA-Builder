using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UsuarioEntity> Usuarios => Set<UsuarioEntity>();
    public DbSet<ClienteEntity> Clientes => Set<ClienteEntity>();
    public DbSet<CategoriaEntity> Categorias => Set<CategoriaEntity>();
    public DbSet<PersonalEntity> Personal => Set<PersonalEntity>();
    public DbSet<ZonaDesfavorableEntity> ZonasDesfavorables => Set<ZonaDesfavorableEntity>();
    public DbSet<ParametrosLiquidacionEntity> ParametrosLiquidacion => Set<ParametrosLiquidacionEntity>();
    public DbSet<NovedadEntity> Novedades => Set<NovedadEntity>();
    public DbSet<ResumenMensualEntity> ResumenesMensuales => Set<ResumenMensualEntity>();
    public DbSet<ResumenPersonalDetalleEntity> ResumenPersonalDetalles => Set<ResumenPersonalDetalleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
