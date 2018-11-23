Imports Wells.Model
Imports Wells.Persistence

Public Class BaseFilter

    Public Enum WellQuery
        All
        OnlyMeasurementWell
        OnlySounding
        ByName
        ZoneA
        ZoneB
        ZoneC
        ZoneD
        Torches
    End Enum

    Private _measurementPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
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


    Private _precipitationPropeties As New Dictionary(Of String, String) From {
        {"Ninguna", "None"},
        {"Milímetros", NameOf(Precipitation.Millimeters)}}

    ReadOnly Property PropertiesNames As List(Of String)
        Get
            Select Case ShowedDatasource
                Case DatasourceType.Measurements
                    Return _measurementPropeties.Keys.ToList
                Case DatasourceType.FLNAAnalysis
                    Return _flnaAnalysisPropeties.Keys.ToList
                Case DatasourceType.SoilAnalysis
                    Return _soilAnalysisPropeties.Keys.ToList
                Case DatasourceType.WaterAnalysis
                    Return _waterAnalysisPropeties.Keys.ToList
                Case DatasourceType.Precipitations
                    Return _precipitationPropeties.Keys.ToList
                Case Else
                    Return New List(Of String)
            End Select
        End Get
    End Property

    Public Enum MeasurementQuery
        FLNADepth
        WaterDepth
        Caudal
        FLNAThickness
        WaterElevation
        FLNAElevation
    End Enum

    Public Enum CriteriaQuery
        ExactValue
        LessThan
        LessOrEqualThan
        GreaterThan
        GreaterOrEqualThan
    End Enum

    Event FilterChanged()

    Event DatasoureceTypeChanged()

    Private _showedDatasource As DatasourceType
    Property ShowedDatasource As DatasourceType
        Get
            Return _showedDatasource
        End Get
        Set
            If _showedDatasource <> Value Then
                _showedDatasource = Value
                My.Settings.ShowedDatasource = Value
                My.Settings.Save()
                RaiseEvent FilterChanged()
                ParameterFilter = 0
                RaiseEvent DatasoureceTypeChanged()
            End If
        End Set
    End Property

    Private _SelectedWellFilterOption As Integer
    Property SelectedWellFilterOption As Integer
        Get
            Return _SelectedWellFilterOption
        End Get
        Set
            If _SelectedWellFilterOption <> Value Then
                _SelectedWellFilterOption = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _SelectedWellName As String
    Property SelectedWellName As String
        Get
            Return _SelectedWellName
        End Get
        Set
            If _SelectedWellName <> Value Then
                _SelectedWellName = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _WellFilter As WellQuery
    Property WellFilter As WellQuery
        Get
            Return _WellFilter
        End Get
        Set
            If _WellFilter <> Value Then
                _WellFilter = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _StartDate As Date
    Property StartDate As Date
        Get
            Return _StartDate
        End Get
        Set
            If _StartDate <> Value Then
                _StartDate = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _EndDate As Date
    Property EndDate As Date
        Get
            Return _EndDate
        End Get
        Set
            If _EndDate <> Value Then
                _EndDate = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _PropertyName As String
    Property PropertyName As String
        Get
            Return _PropertyName
        End Get
        Set
            If _PropertyName <> Value Then
                _PropertyName = Value
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _StringValue As String
    Property StringValue As String
        Get
            Return _doubleValue
        End Get
        Set
            If _StringValue <> Value Then
                _StringValue = Value
                If IsNumeric(Value) Then
                    _doubleValue = Double.Parse(Value, Globalization.NumberStyles.Any)
                Else
                    _StringValue = String.Empty
                End If
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Private _doubleValue As Double

    Private filterFunction As Func(Of Object, String, Double, Boolean)

    Private _parameterFilter As CriteriaQuery
    Property ParameterFilter As CriteriaQuery
        Get
            Return _parameterFilter
        End Get
        Set
            If _parameterFilter <> Value Then
                _parameterFilter = Value
                Select Case _parameterFilter
                    Case CriteriaQuery.ExactValue
                        filterFunction = AddressOf ExactValue
                    Case CriteriaQuery.LessThan
                        filterFunction = AddressOf LessThan
                    Case CriteriaQuery.LessOrEqualThan
                        filterFunction = AddressOf LessOrEqualThan
                    Case CriteriaQuery.GreaterThan
                        filterFunction = AddressOf GreaterThan
                    Case CriteriaQuery.GreaterOrEqualThan
                        filterFunction = AddressOf GreaterOrEqualThan
                End Select
                RaiseEvent FilterChanged()
            End If
        End Set
    End Property

    Protected _repositories As Repositories

    Sub New()
        _repositories = Repositories.Instance
        _showedDatasource = My.Settings.ShowedDatasource
        _StartDate = New Date(2000, 1, 1)
        _EndDate = Today
        filterFunction = AddressOf ExactValue
    End Sub

    Function Apply() As IEnumerable(Of IBusinessObject)
        Select Case ShowedDatasource
            Case DatasourceType.Wells
                Return WellApply(_repositories.Wells)
            Case DatasourceType.Measurements
                Return MeasurementApply(_repositories.Measurements)
            Case DatasourceType.FLNAAnalysis, DatasourceType.SoilAnalysis, DatasourceType.WaterAnalysis
                Return ChemicalAnalysisApply(_repositories.ChemicalAnalysis)
            Case Else
                Return PrecipitationsApply(_repositories.Precipitations)
        End Select
    End Function

    Private Function WellApply(repo As WellsRepository) As List(Of Well)
        Select Case WellFilter
            Case WellQuery.ByName
                If String.IsNullOrEmpty(SelectedWellName) Then
                    Return repo.All
                Else
                    Return {repo.FindName(SelectedWellName)}.ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                Return (From w In repo.All
                        Where w.Type = WellType.MeasurementWell
                        Select w).ToList
            Case WellQuery.OnlySounding
                Return (From w In repo.All
                        Where w.Type = WellType.Sounding
                        Select w).ToList
            Case WellQuery.ZoneA
                Return (From w In repo.All
                        Where Rectangle2D.ZoneA.Contains(w)
                        Select w).ToList
            Case WellQuery.ZoneB
                Return (From w In repo.All
                        Where Rectangle2D.ZoneB.Contains(w)
                        Select w).ToList
            Case WellQuery.ZoneC
                Return (From w In repo.All
                        Where Rectangle2D.ZoneC.Contains(w)
                        Select w).ToList
            Case WellQuery.ZoneD
                Return (From w In repo.All
                        Where Rectangle2D.ZoneD.Contains(w)
                        Select w).ToList
            Case WellQuery.Torches
                Return (From w In repo.All
                        Where Rectangle2D.Torches.Contains(w)
                        Select w).ToList
            Case Else
                Return repo.All
        End Select
    End Function

    Private Function MeasurementApply(repo As MeasurementsRepository) As List(Of Measurement)
        Dim list As List(Of Measurement)
        Select Case WellFilter
            Case WellQuery.ByName
                If String.IsNullOrEmpty(SelectedWellName) Then
                    list = repo.All
                Else
                    list = (From m In repo.All
                            Where m.Well.Name = SelectedWellName
                            Select m).ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                list = (From m In repo.All
                        Where m.Well.Type = WellType.MeasurementWell
                        Select m).ToList
            Case WellQuery.OnlySounding
                list = (From m In repo.All
                        Where m.Well.Type = WellType.Sounding
                        Select m).ToList
            Case WellQuery.ZoneA
                list = (From e In repo.All
                        Where Rectangle2D.ZoneA.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneB
                list = (From e In repo.All
                        Where Rectangle2D.ZoneB.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneC
                list = (From e In repo.All
                        Where Rectangle2D.ZoneC.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneD
                list = (From e In repo.All
                        Where Rectangle2D.ZoneD.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.Torches
                list = (From e In repo.All
                        Where Rectangle2D.Torches.Contains(e.Well)
                        Select e).ToList
            Case Else
                list = repo.All
        End Select

        Dim datedList = (From m In list
                         Where m.RealDate >= StartDate AndAlso m.RealDate <= EndDate
                         Select m).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso _measurementPropeties(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In datedList
                                Where filterFunction(e, _measurementPropeties(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return datedList
        End If

    End Function

    Private Function ChemicalAnalysisApply(repo As ChemicalAnalysisRepository) As List(Of ChemicalAnalysis)
        Dim list As List(Of ChemicalAnalysis)
        Dim dict As Dictionary(Of String, String)
        Select Case ShowedDatasource
            Case DatasourceType.FLNAAnalysis
                list = repo.All.FindAll(Function(a) a.SampleOf = SampleType.FLNA).ToList
                dict = _flnaAnalysisPropeties
            Case DatasourceType.WaterAnalysis
                list = repo.All.FindAll(Function(a) a.SampleOf = SampleType.Water).ToList
                dict = _waterAnalysisPropeties
            Case DatasourceType.SoilAnalysis
                list = repo.All.FindAll(Function(a) a.SampleOf = SampleType.Soil).ToList
                dict = _soilAnalysisPropeties
            Case Else
                Return Nothing
        End Select

        Select Case WellFilter
            Case WellQuery.ByName
                If Not String.IsNullOrEmpty(SelectedWellName) Then
                    list = (From e In list
                            Where e.Well.Name = SelectedWellName
                            Select e).ToList
                End If
            Case WellQuery.OnlyMeasurementWell
                list = (From e In list
                        Where e.Well.Type = WellType.MeasurementWell
                        Select e).ToList
            Case WellQuery.OnlySounding
                list = (From e In list
                        Where e.Well.Type = WellType.Sounding
                        Select e).ToList
            Case WellQuery.ZoneA
                list = (From e In list
                        Where Rectangle2D.ZoneA.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneB
                list = (From e In list
                        Where Rectangle2D.ZoneB.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneC
                list = (From e In list
                        Where Rectangle2D.ZoneC.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.ZoneD
                list = (From e In list
                        Where Rectangle2D.ZoneD.Contains(e.Well)
                        Select e).ToList
            Case WellQuery.Torches
                list = (From e In list
                        Where Rectangle2D.Torches.Contains(e.Well)
                        Select e).ToList
        End Select

        Dim datedList = (From e In list
                         Where e.RealDate >= StartDate AndAlso e.RealDate <= EndDate
                         Select e).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso dict(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In datedList
                                Where filterFunction(e, dict(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return datedList
        End If

    End Function

    Private Function PrecipitationsApply(repo As PrecipitationsRepository) As List(Of Precipitation)

        Dim list = (From e In repo.All
                    Where e.RealDate >= StartDate AndAlso e.RealDate <= EndDate
                    Select e).ToList

        If Not String.IsNullOrEmpty(_PropertyName) AndAlso _precipitationPropeties(_PropertyName) <> "None" AndAlso Not String.IsNullOrEmpty(_StringValue) Then
            Dim filteredList = (From e In list
                                Where filterFunction(e, _precipitationPropeties(_PropertyName), _doubleValue)
                                Select e).ToList

            Return filteredList
        Else
            Return list
        End If

    End Function

    Private Function ExactValue(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue = value
    End Function

    Private Function LessThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue < value
    End Function

    Private Function LessOrEqualThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue <= value
    End Function

    Private Function GreaterThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue > value
    End Function

    Private Function GreaterOrEqualThan(entity As Object, propertyName As String, value As Double) As Boolean
        Dim propertyValue = CDbl(CallByName(entity, propertyName, CallType.Get))
        Return propertyValue >= value
    End Function

End Class
