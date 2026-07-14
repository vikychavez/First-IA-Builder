using LiqPagoEstandar.Core;
using CorePersonal = LiqPagoEstandar.Core.Personal;

namespace LiqPagoEstandar.Data.Entities;

public class PersonalEntity
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public ClienteEntity? Cliente { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public string Apellido { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public CategoriaEntity? Categoria { get; set; }
    public TipoRetiro TipoRetiro { get; set; }
    public string Provincia { get; set; } = string.Empty;
    public decimal HorasMensualesPactadas { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }

    public CorePersonal ToCore()
    {
        if (Categoria is null)
        {
            throw new InvalidOperationException($"Personal {Id} no tiene Categoria cargada.");
        }

        return new CorePersonal(
            Dni,
            ClienteId.ToString(),
            FechaIngreso,
            Apellido,
            Nombre,
            Direccion,
            Telefono,
            Categoria.ToCore(),
            TipoRetiro,
            Provincia,
            HorasMensualesPactadas,
            Activo
        );
    }
}
