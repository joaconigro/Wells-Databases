Imports System.Globalization
Imports System.Windows.Data

Public Class DoubleStringConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNumeric(value) Then
            Dim dbl = CType(value, Double)
            If parameter IsNot Nothing Then
                Return dbl.ToString(parameter.ToString)
            End If
            Return dbl.ToString()
        End If
        Return "0"
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNumeric(value) Then
            Dim str = CType(value, String)
            Dim doubleValue As Double
            Dim ok = Double.TryParse(str, NumberStyles.Any, CultureInfo.CurrentCulture, doubleValue)
            If ok Then
                Return doubleValue
            End If
        End If
        Return 0
    End Function
End Class
