Public Class DateModel
    Property SampleDate As Date
    Property Value As Double

    Sub New(sampleDate As Date, value As Double)
        Me.SampleDate = sampleDate
        Me.Value = value
    End Sub
End Class
