Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data

Public Class HiddenVisibilityToBoolConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If value Is Nothing Then
            Return False
        End If
        If value.GetType Is GetType(Visibility) Then
            Dim vis = CType(value, Visibility)
            Return If(vis = Visibility.Visible, True, False)
        End If
        Return False
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        If value Is Nothing Then
            Return Visibility.Hidden
        End If
        If value.GetType Is GetType(Boolean) Then
            Dim bool = CType(value, Boolean)
            Return If(bool, Visibility.Visible, Visibility.Hidden)
        End If
        Return Visibility.Hidden
    End Function
End Class
