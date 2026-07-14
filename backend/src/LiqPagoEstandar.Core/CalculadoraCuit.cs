namespace LiqPagoEstandar.Core;

public static class CalculadoraCuit
{
    private static readonly int[] Coeficientes = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    public static string Calcular(string dni, string sexo)
    {
        var dniNumerico = new string(dni.Where(char.IsDigit).ToArray());
        if (dniNumerico.Length is 0 or > 8)
        {
            throw new ArgumentException("El DNI debe tener entre 1 y 8 dígitos numéricos.", nameof(dni));
        }

        var dni8 = dniNumerico.PadLeft(8, '0');
        var prefijo = sexo.Equals("F", StringComparison.OrdinalIgnoreCase) ? "27" : "20";

        var verificador = CalcularDigitoVerificador(prefijo, dni8);
        if (verificador == 10)
        {
            // AFIP: cuando el módulo 11 da 10 con el prefijo de sexo (20/27), el CUIT
            // se recalcula forzando el prefijo ambiguo 23 en lugar de rechazar el DNI.
            prefijo = "23";
            verificador = CalcularDigitoVerificador(prefijo, dni8);
            if (verificador == 10)
            {
                verificador = 9;
            }
        }

        return $"{prefijo}{dni8}{verificador}";
    }

    private static int CalcularDigitoVerificador(string prefijo, string dni8)
    {
        var base10 = prefijo + dni8;
        var suma = 0;
        for (var i = 0; i < 10; i++)
        {
            suma += (base10[i] - '0') * Coeficientes[i];
        }

        var resto = suma % 11;
        var verificador = 11 - resto;
        return verificador == 11 ? 0 : verificador;
    }
}
