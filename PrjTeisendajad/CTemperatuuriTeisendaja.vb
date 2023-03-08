Public Class CTemperatuuriTeisendaja
    Implements ITemperatuuriTeisendaja

    Private Function ITemperatuuriTeisendaja_CelciusToFahrenheit(c As Double) As Double Implements ITemperatuuriTeisendaja.CelciusToFahrenheit
        Return (c * 9 / 5) + 32
    End Function

    Private Function ITemperatuuriTeisendaja_FahrenheitToCelsius(f As Double) As Double Implements ITemperatuuriTeisendaja.FahrenheitToCelsius
        Return (f - 32) * 5 / 9
    End Function
End Class
