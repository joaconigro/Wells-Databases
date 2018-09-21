Imports System.ComponentModel
Imports Wells.Model

Public Class BusinessObject
    Implements IBusinessObject

    Public Shared ReadOnly Property NullNumericValue As Double = -9999

    Protected _id As String
    <Browsable(False)>
    Public Property Id As String Implements IBusinessObject.Id
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
        End Set
    End Property

    Sub New()
        _id = Guid.NewGuid.ToString
    End Sub
End Class
