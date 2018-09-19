Public Class Coordinate

    Property X As Double
    Property Y As Double
    Property Z As Double
    Property Latitude As Double
    Property Longitude As Double
    Property HasGeographic As Boolean

    Sub New()
        Me.New(0, 0)
    End Sub

    Sub New(x As Double, y As Double, Optional z As Double = 0, Optional lat As Double = -9999, Optional lng As Double = -9999)
        Me.X = x
        Me.Y = y
        Me.Z = z
        Latitude = lat
        Longitude = lng

        If Latitude = -9999 OrElse Longitude = -9999 Then
            HasGeographic = False
        Else
            HasGeographic = True
        End If
    End Sub

End Class
