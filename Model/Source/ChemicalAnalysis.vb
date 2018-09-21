Imports System.ComponentModel

Public Class ChemicalAnalysis
    Inherits BusinessObject

    <Browsable(False)>
    ReadOnly Property WellId As String
        Get
            Return Well?.Id
        End Get
    End Property

    <DisplayName("Elemento"), Browsable(True)>
    Property Element As String

    <DisplayName("Valor"), Browsable(True)>
    Property Value As Double

    <Browsable(False)>
    Property SampleDate As String

    <Browsable(False)>
    Property SampleOf As SampleType

    <Browsable(False)>
    Property Well As Well

    Private _WellName As String
    <DisplayName("Pozo"), Browsable(True)>
    Property WellName As String
        Get
            If Well IsNot Nothing Then
                Return Well.Name
            Else
                Return _WellName
            End If
        End Get
        Set
            _WellName = Value
        End Set
    End Property

    <DisplayName("Fecha"), Browsable(True)>
    ReadOnly Property RealDate As Date
        Get
            Return Date.ParseExact(SampleDate, "dd/MM/yyyy", Nothing)
        End Get
    End Property

    Sub New()
        MyBase.New()
    End Sub

    Sub New(element As String, value As Double)
        MyBase.New()
        Me.Element = element
        Me.Value = value
    End Sub

    Sub New(element As String, value As Double, well As Well)
        Me.New(element, value)
        Me.Well = well
    End Sub

End Class

Public Enum SampleType
    Water
    FLNA
    Soil
End Enum
