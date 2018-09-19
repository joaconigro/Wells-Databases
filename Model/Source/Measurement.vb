Public Class Measurement
    Inherits BusinessObject

    ReadOnly Property WellId As String
        Get
            Return Well?.Id
        End Get
    End Property

    Property WellName As String
    Property SampleDate As String
    Property FLNADepth As Double
    Property WaterDepth As Double
    Property Caudal As Double
    Property Comment As String
    Property Well As Well

    ReadOnly Property HasFLNA As Boolean
        Get
            Return FLNADepth <> NullNumericValue
        End Get
    End Property

    ReadOnly Property HasWater As Boolean
        Get
            Return WaterDepth <> NullNumericValue
        End Get
    End Property

    ReadOnly Property FLNAThickness As Double
        Get
            If HasFLNA Then
                Return WaterDepth - FLNADepth
            Else
                Return 0
            End If
        End Get
    End Property

    ReadOnly Property WaterElevation As Double
        Get
            Return Well?.Location.Z + Well?.Height - WaterDepth
        End Get
    End Property


    ReadOnly Property FLNAElevation As Double
        Get
            Return Well?.Location.Z + Well?.Height - FLNADepth
        End Get
    End Property
End Class


