Imports System.Globalization
Imports Wells.Model

Public Class DoubleStringConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNumeric(value) Then
            Dim str = CType(value, String)
            Dim doubleValue As Double
            Dim ok = Double.TryParse(str, NumberStyles.Any, CultureInfo.CurrentCulture, doubleValue)
            If ok Then
                Return doubleValue
            End If
        End If
        Return BusinessObject.NullNumericValue
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNumeric(value) Then
            Dim dbl = CType(value, Double)
            Return dbl.ToString("N3")
        End If
        Return String.Empty
    End Function
End Class
