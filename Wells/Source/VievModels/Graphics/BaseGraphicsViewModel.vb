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
    Private _MinimunDate As Date
    Private _RandomGenerator As New Random()
    Private _SelectedSerie As ISeriesView
    Private _MaximunDate As Date
    Protected _SeriesInfo As New Dictionary(Of ISeriesView, SeriesInfo)

    Property MinimunDate As Date
        Get
            Return _MinimunDate
        End Get
        Set
            If Value < _MaximunDate Then
                SetValue(_MinimunDate, Value)
                UpdateSeries()
            End If
        End Set
    End Property

    Property MaximunDate As Date
        Get
            Return _MaximunDate
        End Get
        Set
            If Value > _MinimunDate Then
                SetValue(_MaximunDate, Value)
                UpdateSeries()
            End If
        End Set
    End Property

    Property SelectedSerie As ISeriesView
        Get
            Return _SelectedSerie
        End Get
        Set
            SetValue(_SelectedSerie, Value)
            RaiseCommandUpdates(NameOf(SelectedSerie))
        End Set
    End Property

    ReadOnly Property MinimumY As Double
        Get
            Dim minValues As New List(Of Double)
            For Each s In SeriesCollection
                Dim values = s.Values.GetPoints(s)
                If values.Any Then
                    minValues.Add(values.Min(Function(cp) cp.Y))
                End If
            Next
            If minValues.Any Then
                Return minValues.Min
            End If
            Return Double.NaN
        End Get
    End Property

    ReadOnly Property MaximumY As Double
        Get
            Dim maxValues As New List(Of Double)
            For Each s In SeriesCollection
                Dim values = s.Values.GetPoints(s)
                If values.Any Then
                    maxValues.Add(values.Max(Function(cp) cp.Y))
                End If
            Next
            If maxValues.Any Then
                Return maxValues.Max
            End If
            Return Double.NaN
        End Get
    End Property

    Property XFormatter As Func(Of Double, String)
    Property YFormatter As Func(Of Double, String)
    Property SeriesCollection As SeriesCollection

    Sub New(view As IView)
        MyBase.New(view)
        _MaximunDate = Date.Today
        _MinimunDate = Today.Subtract(TimeSpan.FromDays(180))

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

    Protected Sub OnRemovingSeries(aSeries As ISeriesView)
        _SeriesInfo.Remove(aSeries)
        Dim removeAxis = Not _SeriesInfo.Keys.Where(Function(s) s.ScalesYAt = aSeries.ScalesYAt).Any
        If removeAxis Then
            _Dialog.RemoveAxis(aSeries.ScalesYAt)
        End If
    End Sub

    Protected Function CreateSeriesFromMeasurements(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = Measurement.Properties(parameter).Name
        Dim values = GetMeasurementsValues(well, propertyName, parameter)

        SetAxis(series, "metros")

        series.Values.AddRange(values)
        _SeriesInfo.Add(series, New SeriesInfo(well, propertyName, parameter, AddressOf GetMeasurementsValues))
        Return series
    End Function

    Private Function GetMeasurementsValues(well As Well, propertyName As String, parameter As String) As List(Of DateModel)
        Dim values = (From m In well.Measurements
                      Let param = CType(CallByName(m, propertyName, CallType.Get), Double)
                      Where m.Date >= MinimunDate AndAlso m.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By m.Date Ascending
                      Select New DateModel(m.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If
        Return values
    End Function

    Protected Function CreateSeriesFromSoilAnalyses(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = SoilAnalysis.Properties(parameter).Name
        Dim values = GetSoilAnalysesValues(well, propertyName, parameter)

        Dim units = SoilAnalysis.GetChemicalAnalysisUnits(propertyName)
        If Not String.IsNullOrEmpty(units) Then
            SetAxis(series, units)
        End If

        series.Values.AddRange(values)
        _SeriesInfo.Add(series, New SeriesInfo(well, propertyName, parameter, AddressOf GetSoilAnalysesValues))
        Return series
    End Function

    Private Function GetSoilAnalysesValues(well As Well, propertyName As String, parameter As String) As List(Of DateModel)
        Dim values = (From a In well.SoilAnalyses
                      Let param = CType(CallByName(a, propertyName, CallType.Get), Double)
                      Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By a.Date Ascending
                      Select New DateModel(a.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If
        Return values
    End Function

    Protected Function CreateSeriesFromWaterAnalyses(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = WaterAnalysis.Properties(parameter).Name
        Dim values = GetWaterAnalysesValues(well, propertyName, parameter)

        Dim units = WaterAnalysis.GetChemicalAnalysisUnits(propertyName)
        If Not String.IsNullOrEmpty(units) Then
            SetAxis(series, units)
        End If

        series.Values.AddRange(values)
        _SeriesInfo.Add(series, New SeriesInfo(well, propertyName, parameter, AddressOf GetWaterAnalysesValues))
        Return series
    End Function

    Private Function GetWaterAnalysesValues(well As Well, propertyName As String, parameter As String) As List(Of DateModel)
        Dim values = (From a In well.WaterAnalyses
                      Let param = CType(CallByName(a, propertyName, CallType.Get), Double)
                      Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By a.Date Ascending
                      Select New DateModel(a.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If
        Return values
    End Function

    Protected Function CreateSeriesFromFLNAAnalyses(well As Well, parameter As String) As ISeriesView
        Dim series = CreateLineSeries()
        series.Title = $"{well.Name} - {parameter}"

        Dim propertyName = FLNAAnalysis.Properties(parameter).Name
        Dim values = GetFLNAAnalysesValues(well, propertyName, parameter)

        Dim units = FLNAAnalysis.GetChemicalAnalysisUnits(propertyName)
        If Not String.IsNullOrEmpty(units) Then
            SetAxis(series, units)
        End If

        series.Values.AddRange(values)
        _SeriesInfo.Add(series, New SeriesInfo(well, propertyName, parameter, AddressOf GetFLNAAnalysesValues))
        Return series
    End Function

    Private Function GetFLNAAnalysesValues(well As Well, propertyName As String, parameter As String) As List(Of DateModel)
        Dim values = (From a In well.FLNAAnalyses
                      Let param = CType(CallByName(a, propertyName, CallType.Get), Double)
                      Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                      Order By a.Date Ascending
                      Select New DateModel(a.Date, param)).ToList

        If values.Count < 2 Then
            Throw New Exception($"Hay menos de dos datos de {parameter} para representar en el pozo {well.Name}, por lo tanto no se puede dibujar la línea.")
        End If
        Return values
    End Function

    Protected Function CreateSeriesFromPrecipitations() As ISeriesView
        Dim series = CreateColumnSeries()
        series.Title = "Precipitaciones"

        Dim values = GetPrecipitationsValues()
        SetAxis(series, "mm")

        series.Values.AddRange(values)
        _SeriesInfo.Add(series, New SeriesInfo(AddressOf GetPrecipitationsValues))
        Return series
    End Function

    Private Function GetPrecipitationsValues() As List(Of DateModel)
        Dim values = (From p In RepositoryWrapper.Instance.Precipitations.All
                      Where p.PrecipitationDate >= MinimunDate AndAlso p.PrecipitationDate <= MaximunDate
                      Order By p.PrecipitationDate Ascending
                      Select New DateModel(p.PrecipitationDate, p.Millimeters)).ToList

        If values.Count < 1 Then
            Throw New Exception("No hay datos de precipitaciones para representar, por lo tanto no se dibujará la serie.")
        End If
        Return values
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

    Protected Sub UpdateSeries()
        For Each s In SeriesCollection
            s.Values.Clear()
            s.Values.AddRange(_SeriesInfo(s).GetValues)
        Next
        _Dialog.ResetZoom()
        NotifyPropertyChanged(NameOf(MinimumY))
        NotifyPropertyChanged(NameOf(MaximumY))
    End Sub

    Protected Structure SeriesInfo
        ReadOnly Property Well As Well
        ReadOnly Property PropertyName As String
        ReadOnly Property ParameterName As String
        ReadOnly Property GetWellValuesFunc As Func(Of Well, String, String, List(Of DateModel))
        ReadOnly Property GetPrecipitationValuesFunc As Func(Of List(Of DateModel))
        ReadOnly Property IsFromWell As Boolean

        Sub New(well As Well, propertyName As String, parameterName As String, func As Func(Of Well, String, String, List(Of DateModel)))
            Me.Well = well
            Me.PropertyName = propertyName
            Me.ParameterName = parameterName
            GetWellValuesFunc = func
            GetPrecipitationValuesFunc = Nothing
            IsFromWell = True
        End Sub

        Sub New(func As Func(Of List(Of DateModel)))
            Well = Nothing
            PropertyName = String.Empty
            ParameterName = String.Empty
            GetWellValuesFunc = Nothing
            GetPrecipitationValuesFunc = func
            IsFromWell = False
        End Sub

        Function GetValues() As List(Of DateModel)
            If IsFromWell Then
                Return GetWellValuesFunc.Invoke(Well, PropertyName, ParameterName)
            Else
                Return GetPrecipitationValuesFunc.Invoke
            End If
        End Function
    End Structure
End Class
