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


    Private _Dialog As IGraphicsView
    Private _PiperSchollerPoints As List(Of PiperSchollerData)

    ReadOnly Property PiperSchollerPoints As List(Of PiperSchollerData)
        Get
            Return _PiperSchollerPoints
        End Get
    End Property

    Sub New(wells As IEnumerable(Of Well))
        MyBase.New(Nothing)
        InitializeData(wells)
    End Sub

    Sub New(analises As IEnumerable(Of WaterAnalysis))
        MyBase.New(Nothing)
        InitializeData(analises)
    End Sub

    Private Sub InitializeData(analises As IEnumerable(Of WaterAnalysis))
        _PiperSchollerPoints = New List(Of PiperSchollerData)
        Dim rnd As New Random()

        Dim group = analises.GroupBy(Function(a) a.Well)

        For Each g In group
            For Each a In g
                Dim aColor = New Color() With {
                .B = rnd.Next(0, 50),
                .G = rnd.Next(0, 50),
                .R = rnd.Next(0, 50)}

                _PiperSchollerPoints.Add(CalculatePercentage(a, aColor))
            Next
        Next

    End Sub

    Private Sub InitializeData(wells As IEnumerable(Of Well))
        _PiperSchollerPoints = New List(Of PiperSchollerData)
        Dim rnd As New Random()

        For Each w In wells
            Dim aColor = New Color() With {
                .B = rnd.Next(0, 50),
                .G = rnd.Next(0, 50),
                .R = rnd.Next(0, 50)}

            For Each a In w.WaterAnalyses
                _PiperSchollerPoints.Add(CalculatePercentage(a, aColor))
            Next
        Next

    End Sub

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
        .Label = $"Pozo: {analisys.WellName} - Análisis: {analisys.ToString()}",
        .IsVisible = True
        }

        Return psData
    End Function

    Protected Overrides Sub OnSetView(view As IView)
        MyBase.OnSetView(view)
        _Dialog = CType(view, IGraphicsView)
    End Sub

    Protected Overrides Sub SetValidators()

    End Sub

    Protected Overrides Sub SetCommandUpdates()

    End Sub

    ReadOnly Property SaveChartImageCommand As ICommand = New RelayCommand(Sub()
                                                                               _Dialog.SaveImage()
                                                                           End Sub, Function() True, AddressOf OnError)
End Class
