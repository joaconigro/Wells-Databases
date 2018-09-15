Public Class Well
    Inherits BusinessObject

    Property Name As String
    Property DateMade As String
    Property Location As New Coordinate
    Property Type As WellType
    Property Height As Double
    Property Exists As Boolean

    Property Analysis As New List(Of ChemicalAnalysis)
    Property Measures As New List(Of Measurement)
    Property Links As New List(Of ExternalLink)

    ReadOnly Property FLNAMeasures As List(Of Measurement)
        Get
            Dim list = (From m In Measures
                        Where m.Type = MeasurementType.FLNA
                        Select m).ToList
            Return list
        End Get
    End Property

    ReadOnly Property SuperficialWaterMeasures As List(Of Measurement)
        Get
            Dim list = (From m In Measures
                        Where m.Type = MeasurementType.SuperficialWater
                        Select m).ToList
            Return list
        End Get
    End Property

    ReadOnly Property UndergroundWaterMeasures As List(Of Measurement)
        Get
            Dim list = (From m In Measures
                        Where m.Type = MeasurementType.UndergroundWater
                        Select m).ToList
            Return list
        End Get
    End Property

    ReadOnly Property SoilAnalysis As List(Of ChemicalAnalysis)
        Get
            Dim list = (From a In Analysis
                        Where a.SampleOf = SampleType.Soil
                        Select a).ToList
            Return list
        End Get
    End Property

    ReadOnly Property WaterAnalysis As List(Of ChemicalAnalysis)
        Get
            Dim list = (From a In Analysis
                        Where a.SampleOf = SampleType.Water
                        Select a).ToList
            Return list
        End Get
    End Property

    ReadOnly Property FLNAAnalysis As List(Of ChemicalAnalysis)
        Get
            Dim list = (From a In Analysis
                        Where a.SampleOf = SampleType.FLNA
                        Select a).ToList
            Return list
        End Get
    End Property

    Sub New()
        MyBase.New()
    End Sub

    Sub New(name As String)
        MyBase.New()
        Me.Name = name
    End Sub
End Class

Public Enum WellType
    MeasurementWell
    Sounding
End Enum
