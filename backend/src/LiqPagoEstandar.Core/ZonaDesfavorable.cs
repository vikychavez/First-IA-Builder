namespace LiqPagoEstandar.Core;

public class ZonaDesfavorable
{
    private readonly HashSet<string> _provincias;

    public ZonaDesfavorable(IEnumerable<string> provincias)
    {
        _provincias = new HashSet<string>(provincias, StringComparer.OrdinalIgnoreCase);
    }

    public bool AplicaA(string provincia) => _provincias.Contains(provincia);
}
