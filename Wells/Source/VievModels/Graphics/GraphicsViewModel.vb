Imports Wells.YPFModel
Imports Wells.CorePersistence.Repositories
Imports Wells.ViewBase
Imports LiveCharts
Imports LiveCharts.Definitions.Series

Public Class GraphicsViewModel
    Inherits BaseGraphicsViewModel

    Private _SelectedFromOption As Integer = 0
    Private _SelectedSeriesDataName As String
    Private _SelectedParameterName As String
    Private _Well As Well

    ReadOnly Property FromOptions As New List(Of String) From {"Pozos", "Precipitaciones"}

    ReadOnly Property SeriesDataNames As New List(Of String) From {"Mediciones", "Análisis de FLNA", "Análisis de agua", "Análisis de suelos"}

    Property SelectedFromOption As Integer
        Get
            Return _SelectedFromOption
        End Get
        Set
            SetValue(_SelectedFromOption, Value)
            NotifyPropertyChanged(NameOf(ShowWellOptions))
        End Set
    End Property

    ReadOnly Property ShowWellOptions As Boolean
        Get
            Return If(SelectedFromOption = 0, True, False)
        End Get
    End Property

    Property SelectedSeriesDataName As String
        Get
            Return _SelectedSeriesDataName
        End Get
        Set
            SetValue(_SelectedSeriesDataName, Value)
            NotifyPropertyChanged(NameOf(Parameters))
        End Set
    End Property

    ReadOnly Property Parameters As List(Of String)
        Get
            Select Case _SelectedSeriesDataName
                Case "Mediciones"
                    Return Measurement.DoubleProperties.Keys.ToList
                Case "Análisis de FLNA"
                    Return FLNAAnalysis.DoubleProperties.Keys.ToList
                Case "Análisis de agua"
                    Return WaterAnalysis.DoubleProperties.Keys.ToList
                Case Else
                    Return SoilAnalysis.DoubleProperties.Keys.ToList
            End Select
        End Get
    End Property

    Property SelectedParameterName As String
        Get
            Return _SelectedParameterName
        End Get
        Set
            SetValue(_SelectedParameterName, Value)
        End Set
    End Property

    Private _SelectedWellName As String
    Property SelectedWellName As String
        Get
            Return _SelectedWellName
        End Get
        Set
            SetValue(_SelectedWellName, Value)
            If Not String.IsNullOrEmpty(_SelectedWellName) Then
                _Well = RepositoryWrapper.Instance.Wells.FindByName(_SelectedWellName)
            End If
        End Set
    End Property

    ReadOnly Property WellNames As List(Of String)
        Get
            Return RepositoryWrapper.Instance.Wells.Names
        End Get
    End Property

    Sub New(view As IView)
        MyBase.New(view)
        Initialize()
    End Sub

    Private Sub CreateSerie()
        Dim genericSeries As ISeriesView = Nothing

        Select Case SelectedFromOption
            Case 0
                Select Case _SelectedSeriesDataName
                    Case "Mediciones"
                        genericSeries = CreateSeriesFromMeasurements(_well, SelectedParameterName)
                    Case "Análisis de FLNA"
                        genericSeries = CreateSeriesFromFLNAAnalyses(_well, SelectedParameterName)
                    Case "Análisis de agua"
                        genericSeries = CreateSeriesFromWaterAnalyses(_well, SelectedParameterName)
                    Case Else
                        genericSeries = CreateSeriesFromSoilAnalyses(_well, SelectedParameterName)
                End Select
            Case 1
                genericSeries = CreateSeriesFromPrecipitations()
        End Select

        If genericSeries IsNot Nothing Then
            SeriesCollection.Add(genericSeries)
        End If
    End Sub

    Protected Overrides Sub SetValidators()

    End Sub

    Protected Overrides Sub SetCommandUpdates()
        AddCommands(NameOf(SelectedSerie), {RemoveSeriesCommand})
    End Sub


    ReadOnly Property CreateSeriesCommand As ICommand = New RelayCommand(Sub()
                                                                             CreateSerie()
                                                                         End Sub, Function() True, AddressOf OnError)

    ReadOnly Property RemoveSeriesCommand As ICommand = New RelayCommand(Sub()
                                                                             OnRemovingSeries(SelectedSerie)
                                                                             SeriesCollection.Remove(SelectedSerie)
                                                                             SelectedSerie = Nothing
                                                                         End Sub, Function() SelectedSerie IsNot Nothing, AddressOf OnError)


End Class
