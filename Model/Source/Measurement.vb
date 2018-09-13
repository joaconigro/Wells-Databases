Public Class Measurement
    Inherits BusinessObject

    Property WellId As String
    Property SampleDate As String
    Property FLNADepth As Double
    Property WaterDepth As Double
    Property Caudal As Double
    Property Type As MeasurementType
    Property Comment As String
    Property Well As Well

    ReadOnly Property FLNAThickness As Double
        Get
            Return WaterDepth - FLNADepth
        End Get
    End Property

    ReadOnly Property WaterElevation As Double

    ReadOnly Property FLNAElevation As Double
End Class

Public Enum MeasurementType
    FLNA
    SuperficialWater
    UndergroundWater
End Enum

