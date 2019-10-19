Imports Wells.YPFModel
Imports Wells.ViewBase
Imports LiveCharts
Imports LiveCharts.Definitions.Series

Public Class PremadeGraphicsViewModel
    Inherits BaseGraphicsViewModel

    Private _GraphicSeries As PremadeSeriesInfoCollection
    Private _Well As Well
    ReadOnly Property Title As String

    Sub New(view As IView, well As Well, series As PremadeSeriesInfoCollection)
        MyBase.New(view)
        MinimunDate = New Date(2000, 1, 1)
        Initialize()
        _Well = well
        Title = well.Name
        _GraphicSeries = series
        CreateSeries()
    End Sub

    ReadOnly Property SaveChartImageCommand As ICommand = New RelayCommand(Sub()
                                                                               _Dialog.SaveImage(_Well?.Name)
                                                                           End Sub, Function() True, AddressOf OnError)

    Protected Overrides Sub SetValidators()

    End Sub

    Protected Overrides Sub SetCommandUpdates()

    End Sub

    Private Sub CreateSeries()
        For Each gs In _GraphicSeries.Values
            Dim genericSeries As ISeriesView = Nothing

            If gs.IsFromWell Then
                Select Case gs.ParameterGroup
                    Case "Mediciones"
                        genericSeries = CreateSeriesFromMeasurements(_Well, gs.PropertyDisplayName)
                    Case "Análisis de FLNA"
                        genericSeries = CreateSeriesFromFLNAAnalyses(_Well, gs.PropertyDisplayName)
                    Case "Análisis de agua"
                        genericSeries = CreateSeriesFromWaterAnalyses(_Well, gs.PropertyDisplayName)
                    Case Else
                        genericSeries = CreateSeriesFromSoilAnalyses(_Well, gs.PropertyDisplayName)
                End Select
            Else
                genericSeries = CreateSeriesFromPrecipitations()
            End If

            If genericSeries IsNot Nothing Then
                SeriesCollection.Add(genericSeries)
            End If
        Next
    End Sub
End Class
