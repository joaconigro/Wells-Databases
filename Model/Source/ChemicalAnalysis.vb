Public Class ChemicalAnalysis
    Inherits BusinessObject

    Property WellId As String
    Property Element As String
    Property Value As Double
    Property SampleDate As String
    Property SampleOf As SampleType
    Property Well As Well

    Sub New()
        MyBase.New()
    End Sub

    Sub New(element As String, value As Double)
        MyBase.New()
        Me.Element = element
        Me.Value = value
    End Sub

End Class

Public Enum SampleType
    Water
    FLNA
    Soil
End Enum
