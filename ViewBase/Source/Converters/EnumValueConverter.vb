Imports System.Globalization
Imports Wells.Base.Common
Imports System.Windows.Data

Public Class EnumValueConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Dim anEnum = CType(value, [Enum])
        Return GetEnumDescription(anEnum, False)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim text = value.ToString
        Dim values = EnumDescriptionsToList(targetType, False)
        If values.Contains(text) Then
            Return [Enum].ToObject(targetType, values.IndexOf(text))
        End If
        Return 0
    End Function
End Class
