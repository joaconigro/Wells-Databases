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

    Property SeriesCollection As SeriesCollection

    ReadOnly Property FromOptions As New List(Of String) From {"Pozos", "Precipitaciones"}
    Property SelectedFromOption As Integer = 0

    Property UseMeasurements As Boolean = True

    ReadOnly Property WellNames As List(Of String)
        Get
            Return Repositories.Instance.Wells.Names
        End Get
    End Property

    Property SelectedWellName As String

    Property Formatter As Func(Of Double, String)

    Private _well As Well
    Private WithEvents _SelectedSerieName As CheckedListItem(Of String)
    ReadOnly Property MeasurementPropeties As New Dictionary(Of String, String) From {
        {"Profundidad FLNA", NameOf(Measurement.FLNADepth)},
        {"Profundidad Agua", NameOf(Measurement.WaterDepth)},
        {"Caudal", NameOf(Measurement.Caudal)},
        {"Espesor FLNA", NameOf(Measurement.FLNAThickness)},
        {"Cota Agua", NameOf(Measurement.WaterElevation)},
        {"Cota FLNA", NameOf(Measurement.FLNAElevation)}}

    Property MinimunDate As Date
    Property MaximunDate As Date

    Property Series As New ObservableCollection(Of CheckedListItem(Of String))
    Private _seriesDictionary As New Dictionary(Of String, ISeriesView)

    Property SelectedSerieName As CheckedListItem(Of String)
        Get
            Return _SelectedSerieName
        End Get
        Set
            _SelectedSerieName = Value
            NotifyPropertyChanged(NameOf(SelectedSerieName))
            If SeriesCollection.Any AndAlso Not String.IsNullOrEmpty(_SelectedSerieName?.Item) Then
                _selectedSerie = _seriesDictionary(_SelectedSerieName.Item)
            End If
        End Set
    End Property

    Private WithEvents _selectedSerie As ISeriesView

    Private Sub SetSeriesVisibility(sender As Object, e As PropertyChangedEventArgs) Handles _SelectedSerieName.PropertyChanged
        Dim lSeries = CType(_selectedSerie, LineSeries)
        If CType(sender, CheckedListItem(Of String)).IsChecked Then
            lSeries.Visibility = Visibility.Visible
        Else
            lSeries.Visibility = Visibility.Hidden
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
        Select Case SelectedFromOption
            Case 0

            Case 1
                Dim precipSeries As New LineSeries With {.Name = "Precipitaciones", .LineSmoothness = 0}
                precipSeries.PointGeometry = Nothing
                precipSeries.Fill = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                precipSeries.Values = New ChartValues(Of DateModel)
                Dim values = (From p In Repositories.Instance.Precipitations.All
                              Where p.RealDate >= MinimunDate AndAlso p.RealDate <= MaximunDate
                              Order By p.RealDate Ascending
                              Select New DateModel(p.RealDate, p.Millimeters)).ToList
                precipSeries.Values.AddRange(values)

                Series.Add(New CheckedListItem(Of String)(precipSeries.Name, True))
                _seriesDictionary.Add(precipSeries.Name, precipSeries)
                SeriesCollection.Add(precipSeries)
        End Select
    End Sub

    ReadOnly Property CreateSeriesCommand As ICommand = New Command(Sub()
                                                                        CreateSerie()
                                                                    End Sub, Function() True, AddressOf OnError)

    Protected Overrides Sub ShowErrorMessage(message As String)
        _window.ShowErrorMessageBox(message)
    End Sub
End Class
