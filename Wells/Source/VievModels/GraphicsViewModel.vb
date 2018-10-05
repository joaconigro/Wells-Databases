Imports Wells.Model
Imports Wells.Persistence
Imports LiveCharts
Imports LiveCharts.Wpf
Imports LiveCharts.Configurations
Imports LiveCharts.Definitions.Series
Imports System.Collections.ObjectModel
Imports System.ComponentModel

Public Class GraphicsViewModel
    Inherits BaseViewModel

    Private _window As IView
    Private _randomGenerator As New Random()

    ReadOnly Property FromOptions As New List(Of String) From {"Pozos", "Precipitaciones"}

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

    Private _UseMeasurements As Boolean = True
    Property UseMeasurements As Boolean
        Get
            Return _UseMeasurements
        End Get
        Set
            _UseMeasurements = Value
            NotifyPropertyChanged(NameOf(Parameters))
        End Set
    End Property

    Private ReadOnly _measurementPropeties As New Dictionary(Of String, String) From {
        {"Profundidad FLNA", NameOf(Measurement.FLNADepth)},
        {"Profundidad Agua", NameOf(Measurement.WaterDepth)},
        {"Caudal", NameOf(Measurement.Caudal)},
        {"Espesor FLNA", NameOf(Measurement.FLNAThickness)},
        {"Cota Agua", NameOf(Measurement.WaterElevation)},
        {"Cota FLNA", NameOf(Measurement.FLNAElevation)}}

    Private ReadOnly _chemicalAnalysisPropeties As New Dictionary(Of String, String) From {
        {"Valor", NameOf(ChemicalAnalysis.Value)}}

    ReadOnly Property Parameters As List(Of String)
        Get
            If UseMeasurements Then
                Return _measurementPropeties.Keys.ToList
            Else
                Return _chemicalAnalysisPropeties.Keys.ToList
            End If
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
                If UseMeasurements Then
                    _realParameterName = _measurementPropeties(_SelectedParameterName)
                Else
                    _realParameterName = _chemicalAnalysisPropeties(_SelectedParameterName)
                End If
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
                _well = Repositories.Instance.Wells.FindName(_SelectedWellName)
            End If
        End Set
    End Property

    ReadOnly Property WellNames As List(Of String)
        Get
            Return Repositories.Instance.Wells.Names
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
            If SeriesCollection.Any AndAlso Not String.IsNullOrEmpty(_SelectedSerieName?.Item) Then
                _selectedSerie = _seriesDictionary(_SelectedSerieName.Item)
                CType(RemoveSeriesCommand, Command).RaiseCanExecuteChanged()
            End If
        End Set
    End Property

    Private Sub SetSeriesVisibility(sender As Object, e As PropertyChangedEventArgs) Handles _SelectedSerieName.PropertyChanged
        'Dim lSeries = CType(_selectedSerie, LineSeries)
        If CType(sender, CheckedListItem(Of String)).IsChecked Then
            CallByName(_selectedSerie, "Visibility", CallType.Set, Visibility.Visible)
        Else
            CallByName(_selectedSerie, "Visibility", CallType.Set, Visibility.Hidden)
        End If
    End Sub

    Sub New()
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
                .PointGeometry = Nothing,
                .Fill = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                .Values = New ChartValues(Of DateModel)}

                title = $"{SelectedWellName} - {SelectedParameterName}"
                CType(genericSeries, LineSeries).Title = title
                Dim values As IEnumerable(Of Object)
                If UseMeasurements Then
                    values = (From m In _well.Measurements
                              Let param = CType(CallByName(m, _realParameterName, CallType.Get), Double)
                              Where m.RealDate >= MinimunDate AndAlso m.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                              Order By m.RealDate Ascending
                              Select New DateModel(m.RealDate, param)).ToList
                Else
                    values = (From a In _well.Analysis
                              Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                              Where a.RealDate >= MinimunDate AndAlso a.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                              Order By a.RealDate Ascending
                              Select New DateModel(a.RealDate, param)).ToList
                End If

                If values.Count < 2 Then
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
                Dim values = (From p In Repositories.Instance.Precipitations.All
                              Where p.RealDate >= MinimunDate AndAlso p.RealDate <= MaximunDate
                              Order By p.RealDate Ascending
                              Select New DateModel(p.RealDate, p.Millimeters)).ToList

                If values.Count < 2 Then
                    Exit Sub
                End If
                genericSeries.Values.AddRange(values)
        End Select

        Series.Add(New CheckedListItem(Of String)(title, True))
        _seriesDictionary.Add(title, genericSeries)
        SeriesCollection.Add(genericSeries)
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

    ReadOnly Property CreateSeriesCommand As ICommand = New Command(Sub()
                                                                        CreateSerie()
                                                                    End Sub, Function() True, AddressOf OnError)

    ReadOnly Property RemoveSeriesCommand As ICommand = New Command(Sub()
                                                                        Dim serie = _seriesDictionary(_SelectedSerieName.Item)
                                                                        _seriesDictionary.Remove(_SelectedSerieName.Item)
                                                                        Series.Remove(_SelectedSerieName)
                                                                        SeriesCollection.Remove(serie)
                                                                        _selectedSerie = Nothing
                                                                        CType(RemoveSeriesCommand, Command).RaiseCanExecuteChanged()
                                                                    End Sub, Function() _selectedSerie IsNot Nothing, AddressOf OnError)

    Protected Overrides Sub ShowErrorMessage(message As String)
        _window.ShowErrorMessageBox(message)
    End Sub
End Class
