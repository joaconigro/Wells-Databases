Imports Wells.YPFModel
Imports Wells.CorePersistence.Repositories
Imports Wells.ViewBase
Imports LiveCharts
Imports LiveCharts.Wpf
Imports LiveCharts.Configurations
Imports LiveCharts.Definitions.Series
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports Wells.StandardModel.Models

Public MustInherit Class BaseGraphicsViewModel
    Inherits BaseViewModel

    Protected _Dialog As IGraphicsView

    Private _RandomGenerator As New Random()
    Property MinimunDate As Date
    Property MaximunDate As Date

    Private _SelectedSerie As ISeriesView

    Property SelectedSerie As ISeriesView
        Get
            Return _SelectedSerie
        End Get
        Set
            SetValue(_SelectedSerie, Value)
            RaiseCommandUpdates(NameOf(SelectedSerie))
        End Set
    End Property

    Property XFormatter As Func(Of Double, String)
    Property YFormatter As Func(Of Double, String)
    Property SeriesCollection As SeriesCollection


    Sub New(view As IView)
        MyBase.New(view)
        MaximunDate = Date.Today
        MinimunDate = Today.Subtract(TimeSpan.FromDays(180))

        Dim dateConfig = Mappers.Xy(Of DateModel)()
        dateConfig.X(Function(dm) dm.SampleDate.ToOADate)
        dateConfig.Y(Function(dm) dm.Value)

        XFormatter = New Func(Of Double, String)(Function(d)
                                                     Dim dat = Date.FromOADate(d)
                                                     Return dat.Date.ToShortDateString
                                                 End Function)

        YFormatter = New Func(Of Double, String)(Function(d) d.ToString("N2"))

        SeriesCollection = New SeriesCollection(dateConfig)
        _Dialog = CType(view, IGraphicsView)
    End Sub

    Private Sub SetAxis(aSeries As ISeriesView, units As String)
        Dim axis = _Dialog.GetYAxisIndex(units)
        If axis <> -1 Then
            aSeries.ScalesYAt = axis
        Else
            Dim yAxis As New Axis() With {.Title = units, .Position = AxisPosition.RightTop, .LabelFormatter = YFormatter}
            _Dialog.AddAxis(yAxis)
            axis = _Dialog.GetYAxisIndex(units)
            aSeries.ScalesYAt = axis
        End If
    End Sub

    Protected Function CreateSeriesFromMeasurements(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = Measurement.Properties(parameter).Name
        Dim values = (From m In well.Measurements
                      Let param = CType(CallByName(m, propertyName, CallType.Get), Double)
                      Where m.Date >= MinimunDate AndAlso m.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By m.Date Ascending
                      Select New DateModel(m.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If

        SetAxis(series, "m")

        series.Values.AddRange(values)
        Return series
    End Function

    Protected Function CreateSeriesFromSoilAnalyses(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = SoilAnalysis.Properties(parameter).Name
        Dim values = (From a In well.SoilAnalyses
                      Let param = CType(CallByName(a, parameter, CallType.Get), Double)
                      Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By a.Date Ascending
                      Select New DateModel(a.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If

        Dim units = SoilAnalysis.GetChemicalAnalysisUnits(propertyName)
        If Not String.IsNullOrEmpty(units) Then
            SetAxis(series, units)
        End If

        series.Values.AddRange(values)
        Return series
    End Function

    Protected Function CreateSeriesFromWaterAnalyses(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = WaterAnalysis.Properties(parameter).Name
        Dim values = (From a In well.WaterAnalyses
                      Let param = CType(CallByName(a, parameter, CallType.Get), Double)
                      Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By a.Date Ascending
                      Select New DateModel(a.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If

        Dim units = WaterAnalysis.GetChemicalAnalysisUnits(propertyName)
        If Not String.IsNullOrEmpty(units) Then
            SetAxis(series, units)
        End If

        series.Values.AddRange(values)
        Return series
    End Function


    Protected Function CreateSeriesFromFLNAAnalyses(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = FLNAAnalysis.Properties(parameter).Name
        Dim values = (From a In well.FLNAAnalyses
                      Let param = CType(CallByName(a, parameter, CallType.Get), Double)
                      Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By a.Date Ascending
                      Select New DateModel(a.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If

        Dim units = FLNAAnalysis.GetChemicalAnalysisUnits(propertyName)
        If Not String.IsNullOrEmpty(units) Then
            SetAxis(series, units)
        End If

        series.Values.AddRange(values)
        Return series
    End Function

    Protected Function CreateSeriesFromPrecipitations() As ISeriesView
        Dim series = CreateColumnSeries()
        series.Title = "Precipitaciones"

        Dim values = (From p In RepositoryWrapper.Instance.Precipitations.All
                      Where p.PrecipitationDate >= MinimunDate AndAlso p.PrecipitationDate <= MaximunDate
                      Order By p.PrecipitationDate Ascending
                      Select New DateModel(p.PrecipitationDate, p.Millimeters)).ToList

        If values.Count < 1 Then
            Throw New Exception("No hay datos de precipitaciones para representar, por lo tanto no se dibujará la serie.")
        End If

        series.Values.AddRange(values)

        SetAxis(series, "mm")

        Return series
    End Function

    Private Function CreateLineSeries() As LineSeries
        Dim seriesColor = Color.FromArgb(255, _RandomGenerator.Next(0, 255), _RandomGenerator.Next(0, 255), _RandomGenerator.Next(0, 255))

        Dim lineSeries = New LineSeries() With {
               .LineSmoothness = 0,
               .Stroke = New SolidColorBrush(seriesColor),
               .PointGeometrySize = 8,
               .Fill = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
               .Values = New ChartValues(Of DateModel),
               .StrokeDashArray = New DoubleCollection From {2}}

        Return lineSeries
    End Function

    Private Function CreateColumnSeries() As ColumnSeries
        Dim seriesColor = Color.FromArgb(255, _RandomGenerator.Next(0, 255), _RandomGenerator.Next(0, 255), _RandomGenerator.Next(0, 255))

        Dim columnSeries = New ColumnSeries() With {
                .PointGeometry = Nothing,
                .Fill = New SolidColorBrush(seriesColor),
                .Values = New ChartValues(Of DateModel)}

        Return columnSeries
    End Function

End Class
