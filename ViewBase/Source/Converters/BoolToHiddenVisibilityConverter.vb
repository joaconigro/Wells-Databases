﻿Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data

Public Class BoolToHiddenVisibilityConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        If value Is Nothing Then
            Return Visibility.Hidden
        End If
        If value.GetType Is GetType(Boolean) Then
            Dim bool = CType(value, Boolean)
            Return If(bool, Visibility.Visible, Visibility.Hidden)
        End If
        Return Visibility.Hidden
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
