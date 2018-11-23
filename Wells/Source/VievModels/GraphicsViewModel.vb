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

    'Private _UseMeasurements As Boolean = True
    'Property UseMeasurements As Boolean
    '    Get
    '        Return _UseMeasurements
    '    End Get
    '    Set
    '        _UseMeasurements = Value
    '        NotifyPropertyChanged(NameOf(Parameters))
    '    End Set
    'End Property

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


    Private ReadOnly _measurementPropeties As New Dictionary(Of String, String) From {
        {"Profundidad FLNA", NameOf(Measurement.FLNADepth)},
        {"Profundidad Agua", NameOf(Measurement.WaterDepth)},
        {"Caudal", NameOf(Measurement.Caudal)},
        {"Espesor FLNA", NameOf(Measurement.FLNAThickness)},
        {"Cota Agua", NameOf(Measurement.WaterElevation)},
        {"Cota FLNA", NameOf(Measurement.FLNAElevation)}}

    Private _flnaAnalysisPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
        {"GRO", NameOf(FLNAAnalysis.GRO)},
        {"DRO", NameOf(FLNAAnalysis.DRO)},
        {"MRO", NameOf(FLNAAnalysis.MRO)},
        {"Benceno", NameOf(FLNAAnalysis.Benzene)},
        {"Tolueno", NameOf(FLNAAnalysis.Tolueno)},
        {"Etilbenceno", NameOf(FLNAAnalysis.Ethylbenzene)},
        {"Xilenos", NameOf(FLNAAnalysis.Xylenes)},
        {"C6 - C8", NameOf(FLNAAnalysis.C6_C8)},
        {"C8 - C10", NameOf(FLNAAnalysis.C8_C10)},
        {"C10 - C12", NameOf(FLNAAnalysis.C10_C12)},
        {"C12 - C16", NameOf(FLNAAnalysis.C12_C16)},
        {"C16 - C21", NameOf(FLNAAnalysis.C16_C21)},
        {"C21 - C35", NameOf(FLNAAnalysis.C21_C35)},
        {"C17/Pristano", NameOf(FLNAAnalysis.C17_Pristano)},
        {"C18/Fitano", NameOf(FLNAAnalysis.C18_Fitano)},
        {"Densidad Real", NameOf(FLNAAnalysis.RealDensity)},
        {"Viscosidad Dinámica", NameOf(FLNAAnalysis.DynamicViscosity)}}

    Private _soilAnalysisPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
        {"Humedad", NameOf(SoilAnalysis.Humidity)},
        {"pH", NameOf(SoilAnalysis.PH)},
        {"DRO", NameOf(SoilAnalysis.DRO)},
        {"GRO", NameOf(SoilAnalysis.GRO)},
        {"MRO", NameOf(SoilAnalysis.MRO)},
        {"Hidrocarburos totales(EPA 8015)", NameOf(SoilAnalysis.TotalHydrocarbons_EPA8015)},
        {"Hidrocarburos totales(TNRCC 1005)", NameOf(SoilAnalysis.TotalHydrocarbons_TNRCC1005)},
        {"Aceites y grasas", NameOf(SoilAnalysis.OilsAndFats)},
        {"> C6 - C8 (F. alifática)", NameOf(SoilAnalysis.C6_C8Aliphatic)},
        {"> C8 - C10 (F. alifática)", NameOf(SoilAnalysis.C8_C10Aliphatic)},
        {"> C10 - C12 (F. alifática)", NameOf(SoilAnalysis.C10_C12Aliphatic)},
        {"> C12 - C16 (F. alifática)", NameOf(SoilAnalysis.C12_C16Aliphatic)},
        {"> C16 - C21 (F. alifática)", NameOf(SoilAnalysis.C16_C21Aliphatic)},
        {"> C21 - C35 (F. alifática)", NameOf(SoilAnalysis.C21_C35Aliphatic)},
        {"> C7 - C8 (F. aromática)", NameOf(SoilAnalysis.C7_C8Aromatic)},
        {"> C8 - C10 (F. aromática)", NameOf(SoilAnalysis.C8_C10Aromatic)},
        {"> C10 - C12 (F. aromática)", NameOf(SoilAnalysis.C10_C12Aromatic)},
        {"> C12 - C16 (F. aromática)", NameOf(SoilAnalysis.C12_C16Aromatic)},
        {"> C16 - C21 (F. aromática)", NameOf(SoilAnalysis.C16_C21Aromatic)},
        {"> C21 - C35 (F. aromática)", NameOf(SoilAnalysis.C21_C35Aromatic)},
        {"Benceno", NameOf(SoilAnalysis.Benzene)},
        {"Tolueno", NameOf(SoilAnalysis.Tolueno)},
        {"Etilbenceno", NameOf(SoilAnalysis.Ethylbenzene)},
        {"Xileno (o)", NameOf(SoilAnalysis.XyleneO)},
        {"Xileno (p-m)", NameOf(SoilAnalysis.XylenePM)},
        {"Xileno total", NameOf(SoilAnalysis.TotalXylene)},
        {"Naftaleno", NameOf(SoilAnalysis.Naphthalene)},
        {"Acenafteno", NameOf(SoilAnalysis.Acenafthene)},
        {"Acenaftileno", NameOf(SoilAnalysis.Acenaphthylene)},
        {"Fluoreno", NameOf(SoilAnalysis.Fluorene)},
        {"Antraceno", NameOf(SoilAnalysis.Anthracene)},
        {"Fenantreno", NameOf(SoilAnalysis.Fenanthrene)},
        {"Fluoranteno", NameOf(SoilAnalysis.Fluoranthene)},
        {"Pireno", NameOf(SoilAnalysis.Pyrene)},
        {"Criseno", NameOf(SoilAnalysis.Crysene)},
        {"Benzo(a)antraceno", NameOf(SoilAnalysis.BenzoAAnthracene)},
        {"Benzo(a)pireno", NameOf(SoilAnalysis.BenzoAPyrene)},
        {"Benzo(b)fluoranteno", NameOf(SoilAnalysis.BenzoBFluoranthene)},
        {"Benzo(g,h,i)perileno", NameOf(SoilAnalysis.BenzoGHIPerylene)},
        {"Benzo(k)fluoranteno", NameOf(SoilAnalysis.BenzoKFluoranthene)},
        {"Dibenzo(a,h)antraceno", NameOf(SoilAnalysis.DibenzoAHAnthracene)},
        {"Indeno(1,2,3-cd)pireno", NameOf(SoilAnalysis.Indeno123CDPyrene)},
        {"Arsénico", NameOf(SoilAnalysis.Arsenic)},
        {"Cadmio", NameOf(SoilAnalysis.Cadmium)},
        {"Cobre", NameOf(SoilAnalysis.Copper)},
        {"Cromo total", NameOf(SoilAnalysis.TotalChrome)},
        {"Mercurio", NameOf(SoilAnalysis.Mercury)},
        {"Níquel", NameOf(SoilAnalysis.Nickel)},
        {"Plomo", NameOf(SoilAnalysis.Lead)},
        {"Zinc", NameOf(SoilAnalysis.Zinc)},
        {"Selenio", NameOf(SoilAnalysis.Selenium)}}

    Private _waterAnalysisPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
        {"pH", NameOf(WaterAnalysis.PH)},
        {"Conductividad", NameOf(WaterAnalysis.Conductivity)},
        {"Residuos Secos", NameOf(WaterAnalysis.DryWaste)},
        {"Alcalinidad de Bicarbonato", NameOf(WaterAnalysis.BicarbonateAlkalinity)},
        {"Alcalinidad de Carbonato", NameOf(WaterAnalysis.CarbonateAlkalinity)},
        {"Cloruros", NameOf(WaterAnalysis.Chlorides)},
        {"Nitratos", NameOf(WaterAnalysis.Nitrates)},
        {"Sulfatos", NameOf(WaterAnalysis.Sulfates)},
        {"Calcio", NameOf(WaterAnalysis.Calcium)},
        {"Magnesio", NameOf(WaterAnalysis.Magnesium)},
        {"Sulfuros Totales(HS -)", NameOf(WaterAnalysis.TotalSulfur)},
        {"Potasio", NameOf(WaterAnalysis.Potassium)},
        {"Sodio", NameOf(WaterAnalysis.Sodium)},
        {"Fluoruros", NameOf(WaterAnalysis.Fluorides)},
        {"DRO", NameOf(WaterAnalysis.DRO)},
        {"GRO", NameOf(WaterAnalysis.GRO)},
        {"MRO", NameOf(WaterAnalysis.MRO)},
        {"Hidrocarburos totales(EPA 8015)", NameOf(WaterAnalysis.TotalHydrocarbons_EPA8015)},
        {"Hidrocarburos totales(TNRCC 1005)", NameOf(WaterAnalysis.TotalHydrocarbons_TNRCC1005)},
        {"Benceno", NameOf(WaterAnalysis.Benzene)},
        {"Tolueno", NameOf(WaterAnalysis.Tolueno)},
        {"Etilbenceno", NameOf(WaterAnalysis.Ethylbenzene)},
        {"Xileno (o)", NameOf(WaterAnalysis.XyleneO)},
        {"Xileno (p-m)", NameOf(WaterAnalysis.XylenePM)},
        {"Xileno total", NameOf(WaterAnalysis.TotalXylene)},
        {"Naftaleno", NameOf(WaterAnalysis.Naphthalene)},
        {"Acenafteno", NameOf(WaterAnalysis.Acenafthene)},
        {"Acenaftileno", NameOf(WaterAnalysis.Acenaphthylene)},
        {"Fluoreno", NameOf(WaterAnalysis.Fluorene)},
        {"Antraceno", NameOf(WaterAnalysis.Anthracene)},
        {"Fenantreno", NameOf(WaterAnalysis.Fenanthrene)},
        {"Fluoranteno", NameOf(WaterAnalysis.Fluoranthene)},
        {"Pireno", NameOf(WaterAnalysis.Pyrene)},
        {"Criseno", NameOf(WaterAnalysis.Crysene)},
        {"Benzo(a)antraceno", NameOf(WaterAnalysis.BenzoAAnthracene)},
        {"Benzo(a)pireno", NameOf(WaterAnalysis.BenzoAPyrene)},
        {"Benzo(b)fluoranteno", NameOf(WaterAnalysis.BenzoBFluoranthene)},
        {"Benzo(g,h,i)perileno", NameOf(WaterAnalysis.BenzoGHIPerylene)},
        {"Benzo(k)fluoranteno", NameOf(WaterAnalysis.BenzoKFluoranthene)},
        {"Dibenzo(a,h)antraceno", NameOf(WaterAnalysis.DibenzoAHAnthracene)},
        {"Indeno(1,2,3-cd)pireno", NameOf(WaterAnalysis.Indeno123CDPyrene)},
        {"Arsénico", NameOf(WaterAnalysis.Arsenic)},
        {"Cadmio", NameOf(WaterAnalysis.Cadmium)},
        {"Cobalto", NameOf(WaterAnalysis.Cobalt)},
        {"Cobre", NameOf(WaterAnalysis.Copper)},
        {"Cromo total", NameOf(WaterAnalysis.TotalChrome)},
        {"Mercurio", NameOf(WaterAnalysis.Mercury)},
        {"Níquel", NameOf(WaterAnalysis.Nickel)},
        {"Plomo", NameOf(WaterAnalysis.Lead)},
        {"Zinc", NameOf(WaterAnalysis.Zinc)},
        {"Selenio", NameOf(WaterAnalysis.Selenium)}}

    ReadOnly Property Parameters As List(Of String)
        Get
            Select Case _SelectedSeriesDataName
                Case "Mediciones"
                    Return _measurementPropeties.Keys.ToList
                Case "Análisis de FLNA"
                    Return _flnaAnalysisPropeties.Keys.ToList
                Case "Análisis de agua"
                    Return _waterAnalysisPropeties.Keys.ToList
                Case Else
                    Return _soilAnalysisPropeties.Keys.ToList
            End Select
            'If UseMeasurements Then
            '    Return _measurementPropeties.Keys.ToList
            'Else
            '    Return _chemicalAnalysisPropeties.Keys.ToList
            'End If
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
                        _realParameterName = _measurementPropeties(_SelectedParameterName)
                    Case "Análisis de FLNA"
                        _realParameterName = _flnaAnalysisPropeties(_SelectedParameterName)
                    Case "Análisis de agua"
                        _realParameterName = _waterAnalysisPropeties(_SelectedParameterName)
                    Case Else
                        _realParameterName = _soilAnalysisPropeties(_SelectedParameterName)
                End Select

                'If UseMeasurements Then
                '    _realParameterName = _measurementPropeties(_SelectedParameterName)
                'Else
                '    _realParameterName = _chemicalAnalysisPropeties(_SelectedParameterName)
                'End If
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
                Select Case _SelectedSeriesDataName
                    Case "Mediciones"
                        values = (From m In _well.Measurements
                                  Let param = CType(CallByName(m, _realParameterName, CallType.Get), Double)
                                  Where m.RealDate >= MinimunDate AndAlso m.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                                  Order By m.RealDate Ascending
                                  Select New DateModel(m.RealDate, param)).ToList
                    Case "Análisis de FLNA"
                        values = (From a In _well.Analysis.FindAll(Function(a) a.SampleOf = SampleType.FLNA)
                                  Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                                  Where a.RealDate >= MinimunDate AndAlso a.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                                  Order By a.RealDate Ascending
                                  Select New DateModel(a.RealDate, param)).ToList
                    Case "Análisis de agua"
                        values = (From a In _well.Analysis.FindAll(Function(a) a.SampleOf = SampleType.Water)
                                  Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                                  Where a.RealDate >= MinimunDate AndAlso a.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                                  Order By a.RealDate Ascending
                                  Select New DateModel(a.RealDate, param)).ToList
                    Case Else
                        values = (From a In _well.Analysis.FindAll(Function(a) a.SampleOf = SampleType.Soil)
                                  Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                                  Where a.RealDate >= MinimunDate AndAlso a.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                                  Order By a.RealDate Ascending
                                  Select New DateModel(a.RealDate, param)).ToList
                End Select


                'If UseMeasurements Then
                '    values = (From m In _well.Measurements
                '              Let param = CType(CallByName(m, _realParameterName, CallType.Get), Double)
                '              Where m.RealDate >= MinimunDate AndAlso m.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                '              Order By m.RealDate Ascending
                '              Select New DateModel(m.RealDate, param)).ToList
                'Else
                '    values = (From a In _well.Analysis
                '              Let param = CType(CallByName(a, _realParameterName, CallType.Get), Double)
                '              Where a.RealDate >= MinimunDate AndAlso a.RealDate <= MaximunDate AndAlso param <> BusinessObject.NullNumericValue
                '              Order By a.RealDate Ascending
                '              Select New DateModel(a.RealDate, param)).ToList
                'End If

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
