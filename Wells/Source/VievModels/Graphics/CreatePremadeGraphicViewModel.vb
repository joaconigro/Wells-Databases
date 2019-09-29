Imports Wells
Imports Wells.ViewBase
Imports Wells.YPFModel
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Xml.Serialization

Public Class CreatePremadeGraphicViewModel
    Inherits BaseViewModel

    Private _SelectedFromOption As Integer = 0
    Private _SelectedSeriesDataName As String
    Private _SelectedParameterName As String
    Private _SelectedSerie As PremadeSeriesInfo
    Private _NewCollection As PremadeSeriesInfoCollection
    Private _Title As String
    Private _SelectedGraphic As PremadeSeriesInfoCollection
    ReadOnly Property Series As New ObservableCollection(Of PremadeSeriesInfo)
    ReadOnly Property Graphics As ObservableCollection(Of PremadeSeriesInfoCollection)

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

    Property Title As String
        Get
            Return _Title
        End Get
        Set
            SetValue(_Title, Value)
        End Set
    End Property

    Property SelectedParameterName As String
        Get
            Return _SelectedParameterName
        End Get
        Set
            SetValue(_SelectedParameterName, Value)
        End Set
    End Property

    Property SelectedSerie As PremadeSeriesInfo
        Get
            Return _SelectedSerie
        End Get
        Set
            SetValue(_SelectedSerie, Value)
            RaiseCommandUpdates(NameOf(SelectedSerie))
        End Set
    End Property

    Property SelectedGraphic As PremadeSeriesInfoCollection
        Get
            Return _SelectedGraphic
        End Get
        Set
            SetValue(_SelectedGraphic, Value)
            RaiseCommandUpdates(NameOf(SelectedGraphic))
        End Set
    End Property

    Sub New(view As IView)
        MyBase.New(view)
        Initialize()
        _NewCollection = New PremadeSeriesInfoCollection
        Dim values = ReadPremadeGraphics()
        If values IsNot Nothing AndAlso values.Any Then
            Graphics = New ObservableCollection(Of PremadeSeriesInfoCollection)(values)
        Else
            Graphics = New ObservableCollection(Of PremadeSeriesInfoCollection)
        End If
    End Sub

    Protected Overrides Sub SetValidators()
        AddValidator(NameOf(Title), New EmptyStringValidator("Título"))
    End Sub

    Protected Overrides Sub SetCommandUpdates()
        AddCommands(NameOf(Title), {CreateGraphicCommand})
        AddCommands(NameOf(SelectedSerie), {RemoveSeriesCommand})
        AddCommands(NameOf(SelectedGraphic), {RemoveGraphicCommand})
    End Sub

    Private Function ReadPremadeGraphics() As List(Of PremadeSeriesInfoCollection)
        Dim filename = Path.Combine(Directory.GetCurrentDirectory, "PremadeGraphics.wpg")

        If File.Exists(filename) Then
            Dim serializer As New XmlSerializer(GetType(List(Of PremadeSeriesInfoCollection)))
            Dim entities As List(Of PremadeSeriesInfoCollection) = Nothing
            Using reader As New IO.StreamReader(filename)
                entities = CType(serializer.Deserialize(reader), List(Of PremadeSeriesInfoCollection))
            End Using

            Return entities
        End If
        Return Nothing
    End Function

    Private Sub SavePremadeGraphics()
        Dim filename = Path.Combine(Directory.GetCurrentDirectory, "PremadeGraphics.wpg")

        Dim serializer As New XmlSerializer(GetType(List(Of PremadeSeriesInfoCollection)))
        Dim entities = Graphics.ToList
        Using writer As New StreamWriter(filename)
            serializer.Serialize(writer, entities)
        End Using
    End Sub

    ReadOnly Property CreateSeriesCommand As ICommand = New RelayCommand(Sub()
                                                                             Dim s As PremadeSeriesInfo
                                                                             If ShowWellOptions Then
                                                                                 s = New PremadeSeriesInfo() With {
                                                                                 .IsFromWell = ShowWellOptions,
                                                                                 .ParameterGroup = SelectedSeriesDataName,
                                                                                 .PropertyDisplayName = SelectedParameterName}
                                                                             Else
                                                                                 s = New PremadeSeriesInfo() With {.IsFromWell = ShowWellOptions}
                                                                             End If
                                                                             Series.Add(s)
                                                                             CType(CreateGraphicCommand, RelayCommand).RaiseCanExecuteChanged()
                                                                         End Sub, Function() True, AddressOf OnError)

    ReadOnly Property RemoveSeriesCommand As ICommand = New RelayCommand(Sub()
                                                                             Series.Remove(SelectedSerie)
                                                                             SelectedSerie = Nothing
                                                                             CType(CreateGraphicCommand, RelayCommand).RaiseCanExecuteChanged()
                                                                         End Sub, Function() SelectedSerie IsNot Nothing, AddressOf OnError)

    ReadOnly Property CreateGraphicCommand As ICommand = New RelayCommand(Sub()
                                                                              _NewCollection.Title = Title
                                                                              _NewCollection.Values = Series.ToList
                                                                              Graphics.Add(_NewCollection)
                                                                              Series.Clear()
                                                                              _NewCollection = New PremadeSeriesInfoCollection
                                                                              SavePremadeGraphics()
                                                                          End Sub, Function() Not String.IsNullOrEmpty(Title) AndAlso Series.Any, AddressOf OnError)

    ReadOnly Property RemoveGraphicCommand As ICommand = New RelayCommand(Sub()
                                                                              Graphics.Remove(SelectedGraphic)
                                                                              SelectedGraphic = Nothing
                                                                              SavePremadeGraphics()
                                                                          End Sub, Function() SelectedGraphic IsNot Nothing, AddressOf OnError)
End Class
