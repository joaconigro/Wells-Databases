Imports System.Globalization
Imports System.Windows.Data

Public Class InverseBooleanConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If value.GetType Is GetType(Boolean) Then
            Return Not (CType(value, Boolean))
        End If
        Return False
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If value.GetType Is GetType(Boolean) Then
            Return Not (CType(value, Boolean))
        End If
        Return False
    End Function
End Class
