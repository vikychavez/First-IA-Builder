using LiqPagoEstandar.Core;
using Xunit;

namespace LiqPagoEstandar.Core.Tests;

public class CalculadoraCuitTests
{
    [Theory]
    [InlineData("12345678", "M", "20123456786")]
    [InlineData("30555666", "F", "27305556667")]
    [InlineData("20000009", "M", "23200000099")]
    [InlineData("20000006", "F", "23200000064")]
    [InlineData("5", "M", "20000000052")]
    public void Calcular_DevuelveElCuitConDigitoVerificadorCorrecto(string dni, string sexo, string cuitEsperado)
    {
        var cuit = CalculadoraCuit.Calcular(dni, sexo);

        Assert.Equal(cuitEsperado, cuit);
    }

    [Fact]
    public void Calcular_ConDniVacio_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() => CalculadoraCuit.Calcular(string.Empty, "M"));
    }

    [Fact]
    public void Calcular_ConDniDeMasDeOchoDigitos_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() => CalculadoraCuit.Calcular("123456789", "M"));
    }
}
