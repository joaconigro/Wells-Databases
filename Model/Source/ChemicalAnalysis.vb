Public Class ChemicalAnalysis
    Inherits BusinessObject

    ReadOnly Property WellId As String
        Get
            Return Well?.Id
        End Get
    End Property

    Property Element As String
    Property Value As Double
    Property SampleDate As String
    Property SampleOf As SampleType
    Property Well As Well
    Property WellName As String

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
