Public Class Rectangle2D

    Property X0 As Double
    Property X1 As Double
    Property Y0 As Double
    Property Y1 As Double

    Sub New(x0 As Double, x1 As Double, y0 As Double, y1 As Double)
        Me.X0 = x0
        Me.X1 = x1
        Me.Y0 = y0
        Me.Y1 = y1
    End Sub

    Function Contains(x As Double, y As Double) As Boolean
        If x < X0 Then Return False
        If x > X1 Then Return False
        If y < Y0 Then Return False
        If y > Y1 Then Return False
        Return True
    End Function

    Function Contains(w As Well) As Boolean
        Return Contains(w.X, w.Y)
    End Function

    Shared ReadOnly Property ZoneA As Rectangle2D
        Get
            Return New Rectangle2D(0, 1990, 0, 500)
        End Get
    End Property

    Shared ReadOnly Property ZoneB As Rectangle2D
        Get
            Return New Rectangle2D(0, 1990, 500, 970)
        End Get
    End Property

    Shared ReadOnly Property ZoneC As Rectangle2D
        Get
            Return New Rectangle2D(1990, 4000, 500, 970)
        End Get
    End Property

    Shared ReadOnly Property ZoneD As Rectangle2D
        Get
            Return New Rectangle2D(1990, 4000, 0, 500)
        End Get
    End Property

    Shared ReadOnly Property Torches As Rectangle2D
        Get
            Return New Rectangle2D(0, 4000, 970, 2000)
        End Get
    End Property
End Class
