Imports Wells.YPFModel
Imports Wells.CorePersistence.Repositories
Imports Wells.ViewBase
Imports LiveCharts
Imports LiveCharts.Wpf
Imports LiveCharts.Configurations
Imports LiveCharts.Definitions.Series
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports Xceed.Wpf.Toolkit
Imports Wells.StandardModel.Models

Public Class GraphicsViewModel
    Inherits BaseViewModel

    'Property View As IView
    Private _randomGenerator As New Random()

    ReadOnly Property FromOptions As New List(Of String) From {"Pozos", "Precipitaciones"}

    ReadOnly Property SeriesDataNames As New List(Of String) From {"Mediciones", "Análisis de FLNA", "Análisis de agua", "Análisis de suelos"}


    Private _SelectedFromOption As Integer = 0
    Property SelectedFromOption As Integer
        Get
            Return _SelectedFromOption
        End Get
        Set
            _SelectedFromOption = Value
            NotifyPropertyChanged(NameOf(ShowWellOptions))
        End Set
    End Property

    ReadOnly Property ShowWellOptions As Boolean
        Get
            Return If(SelectedFromOption = 0, True, False)
        End Get
    End Property

    Private _SelectedSeriesDataName As String
    Property SelectedSeriesDataName As String
        Get
            Return _SelectedSeriesDataName
        End Get
        Set
            _SelectedSeriesDataName = Value
            NotifyPropertyChanged(NameOf(Parameters))
        End Set
    End Property

    ReadOnly Property Parameters As List(Of String)
        Get
            Select Case _SelectedSeriesDataName
                Case "Mediciones"
                    Return Measurement.Properties.Keys.ToList
                Case "Análisis de FLNA"
                    Return FLNAAnalysis.Properties.Keys.ToList
                Case "Análisis de agua"
                    Return WaterAnalysis.Properties.Keys.ToList
                Case Else
                    Return SoilAnalysis.Properties.Keys.ToList
            End Select
        End Get
    End Property

    Private _realParameterName As String
    Private _SelectedParameterName As String
    Property SelectedParameterName As String
        Get
            Return _SelectedParameterName
        End Get
        Set
            _SelectedParameterName = Value
            If Not String.IsNullOrEmpty(_SelectedParameterName) Then
                Select Case _SelectedSeriesDataName
                    Case "Mediciones"
                        _realParameterName = Measurement.Properties(_SelectedParameterName).Name
                    Case "Análisis de FLNA"
                        _realParameterName = FLNAAnalysis.Properties(_SelectedParameterName).Name
                    Case "Análisis de agua"
                        _realParameterName = WaterAnalysis.Properties(_SelectedParameterName).Name
                    Case Else
                        _realParameterName = SoilAnalysis.Properties(_SelectedParameterName).Name
                End Select
            End If
        End Set
    End Property

    Private _SelectedWellName As String
    Property SelectedWellName As String
        Get
            Return _SelectedWellName
        End Get
        Set
            _SelectedWellName = Value
            If Not String.IsNullOrEmpty(_SelectedWellName) Then
                _well = RepositoryWrapper.Instance.Wells.FindByName(_SelectedWellName)
            End If
        End Set
    End Property

    ReadOnly Property WellNames As List(Of String)
        Get
            Return RepositoryWrapper.Instance.Wells.Names
        End Get
    End Property

    Private _well As Well

    Property MinimunDate As Date
    Property MaximunDate As Date

    Private WithEvents _selectedSerie As ISeriesView
    Property Formatter As Func(Of Double, String)
    Private WithEvents _SelectedSerieName As CheckedListItem(Of String)
    Property Series As New ObservableCollection(Of CheckedListItem(Of String))
    Private _seriesDictionary As New Dictionary(Of String, ISeriesView)
    Property SeriesCollection As SeriesCollection

    Property SelectedSerieName As CheckedListItem(Of String)
        Get
            Return _SelectedSerieName
        End Get
        Set
            _SelectedSerieName = Value
            NotifyPropertyChanged(NameOf(SelectedSerieName))
            If SeriesCollection.Any AndAlso Not String.IsNullOrEmpty(_SelectedSerieName?.Name) Then
                _selectedSerie = _seriesDictionary(_SelectedSerieName.Name)
                CType(RemoveSeriesCommand, RelayCommand).RaiseCanExecuteChanged()
            End If
        End Set
    End Property

    Private Sub SetSeriesVisibility(sender As Object, e As PropertyChangedEventArgs) Handles _SelectedSerieName.PropertyChanged
        'Dim lSeries = CType(_selectedSerie, LineSeries)
        If _selectedSerie IsNot Nothing Then
            If CType(sender, CheckedListItem(Of String)).IsChecked Then
                CallByName(_selectedSerie, "Visibility", CallType.Set, Visibility.Visible)
            Else
                CallByName(_selectedSerie, "Visibility", CallType.Set, Visibility.Hidden)
            End If
        End If

    End Sub

    Sub New()
        MyBase.New(Nothing)
        MaximunDate = Date.Today
        MinimunDate = Date.FromOADate(Date.Today.ToOADate - 180)

        Dim dateConfig = Mappers.Xy(Of DateModel)()
        dateConfig.X(Function(dm) dm.SampleDate.ToOADate)
        dateConfig.Y(Function(dm) dm.Value)

        Formatter = New Func(Of Double, String)(Function(d)
                                                    Dim dat = Date.FromOADate(d)
                                                    Return dat.Date.ToShortDateString
                                                End Function)

        SeriesCollection = New SeriesCollection(dateConfig)
    End Sub


    Private Sub CreateSerie()
        Dim genericSeries As ISeriesView = Nothing
        Dim seriesColor = Color.FromArgb(255, _randomGenerator.Next(0, 255), _randomGenerator.Next(0, 255), _randomGenerator.Next(0, 255))
        Dim title As String = ""

        Select Case SelectedFromOption
            Case 0
                genericSeries = New LineSeries() With {
                .LineSmoothness = 0,
                .Stroke = New SolidColorBrush(seriesColor),
                .PointGeometrySize = 8,
                .Fill = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                .Values = New ChartValues(Of DateModel),
                .StrokeDashArray = New DoubleCollection From {2}}

                title = $"{SelectedWellName} - {SelectedParameterName}"
                CType(genericSeries, LineSeries).Title = title
                Dim values As IEnumerable(Of Object)
                Select Case _SelectedSeriesDataName
                    Case "Mediciones"
                        values = (From m In _well.Measurements
                                  Let param = CType(CallByName(m, _realParameterName, CallType.Get), Double)
                                  Where m.Date >= MinimunDate AndAlso m.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                                  Order By m.Date Ascending
                                  Select New DateModel(m.Date, param)).ToList
                    Case "Análisis de FLNA"
                        values = (From a In _well.FLNAAnalyses
                                  Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                                  Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                                  Order By a.Date Ascending
                                  Select New DateModel(a.Date, param)).ToList
                    Case "Análisis de agua"
                        values = (From a In _well.WaterAnalyses
                                  Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                                  Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                                  Order By a.Date Ascending
                                  Select New DateModel(a.Date, param)).ToList
                    Case Else
                        values = (From a In _well.SoilAnalyses
                                  Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                                  Where a.Date >= MinimunDate AndAlso a.Date <= MaximunDate AndAlso param <> BusinessObject.NumericNullValue
                                  Order By a.Date Ascending
                                  Select New DateModel(a.Date, param)).ToList
                End Select


                If values.Count < 2 Then
                    View.ShowErrorMessageBox("Hay menos de dos datos para representar, por lo tanto no se puede dibujar la línea.")
                    Exit Sub
                End If
                genericSeries.Values.AddRange(values)

            Case 1
                genericSeries = New ColumnSeries() With {
                .PointGeometry = Nothing,
                .Fill = New SolidColorBrush(seriesColor),
                .Values = New ChartValues(Of DateModel)}

                title = "Precipitaciones"
                CType(genericSeries, ColumnSeries).Title = title
                Dim values = (From p In RepositoryWrapper.Instance.Precipitations.All
                              Where p.PrecipitationDate >= MinimunDate AndAlso p.PrecipitationDate <= MaximunDate
                              Order By p.PrecipitationDate Ascending
                              Select New DateModel(p.PrecipitationDate, p.Millimeters)).ToList

                If values.Count < 1 Then
                    View.ShowErrorMessageBox("No hay datos para representar, por lo tanto no se dibujará la serie.")
                    Exit Sub
                End If
                genericSeries.Values.AddRange(values)
        End Select

        Dim axis = CType(View, IGraphicsView).GetYAxis(SelectedParameterName)
        If axis <> -1 Then
            genericSeries.ScalesYAt = axis
        Else
            Dim yAxis As New LiveCharts.Wpf.Axis() With {.Title = SelectedParameterName, .Position = AxisPosition.RightTop}
            CType(View, IGraphicsView).AddAxis(yAxis)
            axis = CType(View, IGraphicsView).GetYAxis(SelectedParameterName)
            genericSeries.ScalesYAt = axis
        End If

        Series.Add(New CheckedListItem(Of String)(title, True))
        _seriesDictionary.Add(title, genericSeries)
        SeriesCollection.Add(genericSeries)
    End Sub

    Protected Overrides Sub SetValidators()
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub SetCommandUpdates()
        Throw New NotImplementedException()
    End Sub

    'Private Function GetValues(paramName As String) As IEnumerable(Of Object)
    '    Dim objects As IEnumerable(Of IBusinessObject)
    '    If UseMeasurements Then
    '        objects = _well.Measurements
    '    Else
    '        objects = _well.Analysis
    '    End If
    '    Dim values = (From p In objects
    '                  Let pDate = CType(CallByName(p, "RealDate", CallType.Get), Date)
    '                  Where pDate >= MinimunDate AndAlso pDate <= MaximunDate
    '                  Order By pDate Ascending
    '                  Select New DateModel(pDate, CType(CallByName(p, paramName, CallType.Get), Double))).ToList
    '    Return values
    'End Function

    'Private Function GetValues(repo As IRepository(Of IBusinessObject), paramName As String) As IEnumerable(Of Object)
    '    Dim values = (From p In repo.All
    '                  Let pDate = CType(CallByName(p, "RealDate", CallType.Get), Date)
    '                  Where pDate >= MinimunDate AndAlso pDate <= MaximunDate
    '                  Order By pDate Ascending
    '                  Select New DateModel(pDate, CType(CallByName(p, paramName, CallType.Get), Double))).ToList
    '    Return values
    'End Function

    ReadOnly Property CreateSeriesCommand As ICommand = New RelayCommand(Sub()
                                                                             CreateSerie()
                                                                         End Sub, Function() True, AddressOf OnError)

    ReadOnly Property RemoveSeriesCommand As ICommand = New RelayCommand(Sub()
                                                                             Dim serie = _seriesDictionary(_SelectedSerieName.Name)
                                                                             _seriesDictionary.Remove(_SelectedSerieName.Name)
                                                                             Series.Remove(_SelectedSerieName)
                                                                             SeriesCollection.Remove(serie)
                                                                             _selectedSerie = Nothing
                                                                             CType(RemoveSeriesCommand, RelayCommand).RaiseCanExecuteChanged()
                                                                         End Sub, Function() _selectedSerie IsNot Nothing, AddressOf OnError)


End Class
