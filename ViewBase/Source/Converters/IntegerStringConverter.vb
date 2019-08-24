Imports System.Globalization
Imports System.Windows.Data

Public Class IntegerStringConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If IsNumeric(value) Then
            Dim int = CType(value, Integer)
            If parameter IsNot Nothing Then
                Return int.ToString(parameter.ToString)
            End If
            Return int.ToString()
        End If
        Return "0"
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If IsNumeric(value) Then
            Dim str = CType(value, String)
            Dim integerValue As Integer
            Dim ok = Integer.TryParse(str, NumberStyles.Any, CultureInfo.CurrentCulture, integerValue)
            If ok Then
                Return integerValue
            End If
        End If
        Return 0
    End Function
End Class
