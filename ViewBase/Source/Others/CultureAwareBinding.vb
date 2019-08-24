Imports System.Globalization
Imports System.Windows.Data

Public Class CultureAwareBinding
    Inherits Binding

    Sub New()
        MyBase.New()
        ConverterCulture = CultureInfo.CurrentCulture
    End Sub

    Sub New(path As String)
        MyBase.New(path)
        ConverterCulture = CultureInfo.CurrentCulture
    End Sub
End Class
