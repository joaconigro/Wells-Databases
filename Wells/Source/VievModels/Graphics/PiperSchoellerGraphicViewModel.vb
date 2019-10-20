Imports Wells.ViewBase
Imports Wells.YPFModel

Public Class PiperSchoellerGraphicViewModel
    Inherits BaseViewModel

    Private Const CalciumMeq As Double = 20
    Private Const PotassiumMeq As Double = 39
    Private Const SodiumMeq As Double = 23
    Private Const MagnesiumMeq As Double = 12.15
    Private Const CarbonateMeq As Double = 61
    Private Const SulfatesMeq As Double = 48
    Private Const ChloridesMeq As Double = 35.5

    Dim _Random As New Random()

    Private _Dialog As IPiperSchoellerGraphicView
    Private _PiperSchollerPoints As List(Of PiperSchollerData)
    Private _SelectedPoint As PiperSchollerData

    ReadOnly Property PiperSchollerPoints As List(Of PiperSchollerData)
        Get
            Return _PiperSchollerPoints
        End Get
    End Property

    Property SelectedPoint As PiperSchollerData
        Get
            Return _SelectedPoint
        End Get
        Set
            SetValue(_SelectedPoint, Value)
        End Set
    End Property

    Sub New(wells As IEnumerable(Of Well))
        MyBase.New(Nothing)
        Initialize()
        InitializeData(wells)
    End Sub

    Sub New(analises As IEnumerable(Of WaterAnalysis))
        MyBase.New(Nothing)
        Initialize()
        InitializeData(analises)
    End Sub

    Private Sub InitializeData(analises As IEnumerable(Of WaterAnalysis))
        _PiperSchollerPoints = New List(Of PiperSchollerData)

        Dim group = analises.GroupBy(Function(a) a.Well)

        For Each g In group
            For Each a In g
                Dim aColor = GetRandomColor()
                _PiperSchollerPoints.Add(CalculatePercentage(a, aColor))
            Next
        Next

    End Sub

    Private Sub InitializeData(wells As IEnumerable(Of Well))
        _PiperSchollerPoints = New List(Of PiperSchollerData)

        For Each w In wells
            Dim aColor = GetRandomColor()
            For Each a In w.WaterAnalyses
                _PiperSchollerPoints.Add(CalculatePercentage(a, aColor))
            Next
        Next

    End Sub

    Private Function GetRandomColor() As Color
        Dim names = [Enum].GetValues(GetType(System.Drawing.KnownColor))
        Dim randomColorName = names(_Random.Next(names.Length))
        Dim randomColor = System.Drawing.Color.FromKnownColor(randomColorName)
        Return Color.FromArgb(randomColor.A, randomColor.R, randomColor.G, randomColor.B)
    End Function

    Private Function CalculatePercentage(analisys As WaterAnalysis, color As Color) As PiperSchollerData
        Dim cal = analisys.Calcium / CalciumMeq
        Dim pot = analisys.Potassium / PotassiumMeq
        Dim sod = analisys.Sodium / SodiumMeq
        Dim mag = analisys.Magnesium / MagnesiumMeq
        Dim totalCations = cal + pot + sod + mag

        Dim carb = analisys.CarbonateAlkalinity / CarbonateMeq
        Dim sulf = analisys.Sulfates / SulfatesMeq
        Dim chlo = analisys.Chlorides / ChloridesMeq
        Dim totalAnions = carb + sulf + chlo

        Dim psData As New PiperSchollerData() With {
        .Calcium = cal / totalCations * 100,
        .Magnesium = mag / totalCations * 100,
        .PotassiumAndSodium = (sod + pot) / totalCations * 100,
        .Carbonate = carb / totalAnions * 100,
        .Chlorides = chlo / totalAnions * 100,
        .Sulfates = sulf / totalAnions * 100,
        .PointColor = color,
        .Label = $"{analisys.WellName} ({analisys.ToString()})",
        .IsVisible = True
        }

        Return psData
    End Function

    Protected Overrides Sub OnSetView(view As IView)
        MyBase.OnSetView(view)
        _Dialog = CType(view, IPiperSchoellerGraphicView)
    End Sub

    Protected Overrides Sub SetValidators()

    End Sub

    Protected Overrides Sub SetCommandUpdates()
        AddCommands(NameOf(SelectedPoint), {ChangeColorCommand})
    End Sub

    ReadOnly Property ChangeColorCommand As ICommand = New RelayCommand(Sub()
                                                                            Dim currentColor = System.Drawing.Color.FromArgb(SelectedPoint.PointColor.A,
                                                                                                                             SelectedPoint.PointColor.R,
                                                                                                                             SelectedPoint.PointColor.G,
                                                                                                                             SelectedPoint.PointColor.B)
                                                                            SelectedPoint.PointColor = _Dialog.ShowColorDialog(currentColor)
                                                                            _Dialog.CreateGraphics()
                                                                        End Sub, Function() SelectedPoint IsNot Nothing, AddressOf OnError)

    ReadOnly Property SaveImageCommand As ICommand = New RelayCommand(Sub()
                                                                          _Dialog.SaveImage()
                                                                      End Sub, Function() True, AddressOf OnError)
End Class
