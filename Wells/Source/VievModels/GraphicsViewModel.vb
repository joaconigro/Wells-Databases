Imports System
Imports System.Windows.Controls
Imports System.Windows.Media

Imports Wells.Model

Imports Wells.Persistence
Imports LiveCharts
Imports LiveCharts.Wpf
Imports LiveCharts.Configurations
Imports System.Runtime.Serialization

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

    Property Formatter As Func(Of Date, String)

    Private _well As Well

    Sub New()
        Dim dateConfig = Mappers.Xy(Of DateModel)()
        dateConfig.X(Function(dm) CDbl(dm.SampleDate.Ticks / TimeSpan.FromDays(1).Ticks))
        dateConfig.Y(Function(dm) dm.Value)

        Formatter = New Func(Of Date, String)(Function(d) d.Date.ToShortDateString)

        SeriesCollection = New SeriesCollection(dateConfig)
    End Sub


    Private Sub CreateSerie()
        Select Case SelectedFromOption
            Case 0

            Case 1
                Dim precipSeries As New LineSeries With {.Name = "Precipitaciones", .LineSmoothness = 0}
                precipSeries.PointGeometry = Nothing
                precipSeries.Fill = New SolidColorBrush(Color.FromArgb(0, 0, 0, 0))
                Dim limit As New Date(2017, 1, 1)
                precipSeries.Values = New ChartValues(Of DateModel)
                Dim values = (From p In Repositories.Instance.Precipitations.All
                              Where p.RealDate > limit
                              Order By p.RealDate Ascending
                              Select New DateModel(p.RealDate, p.Millimeters)).ToList
                precipSeries.Values.AddRange(values)

                SeriesCollection.Add(precipSeries)
        End Select
    End Sub

    ReadOnly Property CreateSeriesCommand As ICommand = New Command(Sub()
                                                                        CreateSerie()
                                                                    End Sub, Function() True, AddressOf OnError)

    Protected Overrides Sub ShowErrorMessage(message As String)
        _window.ShowErrorMessageBox(message)
    End Sub

    Public Class DateModel
        Property SampleDate As Date
        Property Value As Double

        Sub New(sampleDate As Date, value As Double)
            Me.SampleDate = sampleDate
            Me.Value = value
        End Sub
    End Class
End Class
