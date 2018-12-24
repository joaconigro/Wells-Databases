Imports System.ComponentModel
Imports Wells.Model.ReflectionExtension

Public Class WaterAnalysis
    Inherits ChemicalAnalysis

    Sub New()
        MyBase.New()
        SampleOf = SampleType.Water
    End Sub

    Sub New(well As Well)
        MyBase.New(well)
        SampleOf = SampleType.Water
    End Sub

    <DisplayName("Pozo"), Browsable(True)>
    Overrides Property WellName As String
        Get
            If Well IsNot Nothing Then
                Return Well.Name
            Else
                Return _WellName
            End If
        End Get
        Set
            _WellName = Value
        End Set
    End Property

    <DisplayName("Fecha"), Browsable(True)>
    Overrides ReadOnly Property RealDate As Date
        Get
            Return Date.ParseExact(SampleDate, "dd/MM/yyyy", Nothing)
        End Get
    End Property

    <DisplayName("pH"), Browsable(True)>
    Property PH As Double

    <DisplayName("Conductividad"), Browsable(True)>
    Property Conductivity As Double

    <DisplayName("Residuos Secos"), Browsable(True)>
    Property DryWaste As Double

    <DisplayName("Alcalinidad de Bicarbonato"), Browsable(True)>
    Property BicarbonateAlkalinity As Double

    <DisplayName("Alcalinidad de Carbonato"), Browsable(True)>
    Property CarbonateAlkalinity As Double

    <DisplayName("Cloruros"), Browsable(True)>
    Property Chlorides As Double

    <DisplayName("Nitratos"), Browsable(True)>
    Property Nitrates As Double

    <DisplayName("Sulfatos"), Browsable(True)>
    Property Sulfates As Double

    <DisplayName("Calcio"), Browsable(True)>
    Property Calcium As Double

    <DisplayName("Magnesio"), Browsable(True)>
    Property Magnesium As Double

    <DisplayName("Sulfuros Totales (HS-)"), Browsable(True)>
    Property TotalSulfur As Double

    <DisplayName("Potasio"), Browsable(True)>
    Property Potassium As Double

    <DisplayName("Sodio"), Browsable(True)>
    Property Sodium As Double

    <DisplayName("Fluoruros"), Browsable(True)>
    Property Fluorides As Double

    <DisplayName("DRO"), Browsable(True)>
    Property DRO As Double

    <DisplayName("GRO"), Browsable(True)>
    Property GRO As Double

    <DisplayName("MRO"), Browsable(True)>
    Property MRO As Double

    <DisplayName("Hidrocarburos totales (EPA 8015)"), Browsable(True)>
    Property TotalHydrocarbons_EPA8015 As Double

    <DisplayName("Hidrocarburos totales (TNRCC 1005)"), Browsable(True)>
    Property TotalHydrocarbons_TNRCC1005 As Double

    <DisplayName("Benceno"), Browsable(True)>
    Property Benzene As Double

    <DisplayName("Tolueno"), Browsable(True)>
    Property Tolueno As Double

    <DisplayName("Etilbenceno"), Browsable(True)>
    Property Ethylbenzene As Double

    <DisplayName("Xileno (o)"), Browsable(True)>
    Property XyleneO As Double

    <DisplayName("Xileno (p-m)"), Browsable(True)>
    Property XylenePM As Double

    <DisplayName("Xileno total"), Browsable(True)>
    Property TotalXylene As Double

    <DisplayName("Naftaleno"), Browsable(True)>
    Property Naphthalene As Double

    <DisplayName("Acenafteno"), Browsable(True)>
    Property Acenafthene As Double

    <DisplayName("Acenaftileno"), Browsable(True)>
    Property Acenaphthylene As Double

    <DisplayName("Fluoreno"), Browsable(True)>
    Property Fluorene As Double

    <DisplayName("Antraceno"), Browsable(True)>
    Property Anthracene As Double

    <DisplayName("Fenantreno"), Browsable(True)>
    Property Fenanthrene As Double

    <DisplayName("Fluoranteno"), Browsable(True)>
    Property Fluoranthene As Double

    <DisplayName("Pireno"), Browsable(True)>
    Property Pyrene As Double

    <DisplayName("Benzo(a)antraceno"), Browsable(True)>
    Property BenzoAAnthracene As Double

    <DisplayName("Criseno"), Browsable(True)>
    Property Crysene As Double

    <DisplayName("Benzo(a)pireno"), Browsable(True)>
    Property BenzoAPyrene As Double

    <DisplayName("Benzo(b)fluoranteno"), Browsable(True)>
    Property BenzoBFluoranthene As Double

    <DisplayName("Benzo(g,h,i)perileno"), Browsable(True)>
    Property BenzoGHIPerylene As Double

    <DisplayName("Benzo(k)fluoranteno"), Browsable(True)>
    Property BenzoKFluoranthene As Double

    <DisplayName("Dibenzo(a,h)antraceno"), Browsable(True)>
    Property DibenzoAHAnthracene As Double

    <DisplayName("Indeno(1,2,3-cd)pireno"), Browsable(True)>
    Property Indeno123CDPyrene As Double

    <DisplayName("Arsénico"), Browsable(True)>
    Property Arsenic As Double

    <DisplayName("Cadmio"), Browsable(True)>
    Property Cadmium As Double

    <DisplayName("Cobalto"), Browsable(True)>
    Property Cobalt As Double

    <DisplayName("Cobre"), Browsable(True)>
    Property Copper As Double

    <DisplayName("Cromo total"), Browsable(True)>
    Property TotalChrome As Double

    <DisplayName("Mercurio"), Browsable(True)>
    Property Mercury As Double

    <DisplayName("Níquel"), Browsable(True)>
    Property Nickel As Double

    <DisplayName("Plomo"), Browsable(True)>
    Property Lead As Double

    <DisplayName("Selenio"), Browsable(True)>
    Property Selenium As Double

    <DisplayName("Zinc"), Browsable(True)>
    Property Zinc As Double

    Public Overrides Function GetChemicalAnalysisType(propertyName As String) As ChemicalAnalysisType
        Return WaterAnalysisTypes(propertyName)
    End Function

    Shared ReadOnly Property Properties As New Dictionary(Of String, String) From {
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.PH)), NameOf(WaterAnalysis.PH)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Conductivity)), NameOf(WaterAnalysis.Conductivity)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.DryWaste)), NameOf(WaterAnalysis.DryWaste)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.BicarbonateAlkalinity)), NameOf(WaterAnalysis.BicarbonateAlkalinity)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.CarbonateAlkalinity)), NameOf(WaterAnalysis.CarbonateAlkalinity)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Chlorides)), NameOf(WaterAnalysis.Chlorides)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Nitrates)), NameOf(WaterAnalysis.Nitrates)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Sulfates)), NameOf(WaterAnalysis.Sulfates)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Calcium)), NameOf(WaterAnalysis.Calcium)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Magnesium)), NameOf(WaterAnalysis.Magnesium)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.TotalSulfur)), NameOf(WaterAnalysis.TotalSulfur)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Potassium)), NameOf(WaterAnalysis.Potassium)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Sodium)), NameOf(WaterAnalysis.Sodium)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Fluorides)), NameOf(WaterAnalysis.Fluorides)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.DRO)), NameOf(WaterAnalysis.DRO)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.GRO)), NameOf(WaterAnalysis.GRO)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.MRO)), NameOf(WaterAnalysis.MRO)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.TotalHydrocarbons_EPA8015)), NameOf(WaterAnalysis.TotalHydrocarbons_EPA8015)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.TotalHydrocarbons_TNRCC1005)), NameOf(WaterAnalysis.TotalHydrocarbons_TNRCC1005)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Benzene)), NameOf(WaterAnalysis.Benzene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Tolueno)), NameOf(WaterAnalysis.Tolueno)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Ethylbenzene)), NameOf(WaterAnalysis.Ethylbenzene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.XyleneO)), NameOf(WaterAnalysis.XyleneO)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.XylenePM)), NameOf(WaterAnalysis.XylenePM)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.TotalXylene)), NameOf(WaterAnalysis.TotalXylene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Naphthalene)), NameOf(WaterAnalysis.Naphthalene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Acenafthene)), NameOf(WaterAnalysis.Acenafthene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Acenaphthylene)), NameOf(WaterAnalysis.Acenaphthylene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Fluorene)), NameOf(WaterAnalysis.Fluorene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Anthracene)), NameOf(WaterAnalysis.Anthracene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Fenanthrene)), NameOf(WaterAnalysis.Fenanthrene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Fluoranthene)), NameOf(WaterAnalysis.Fluoranthene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Pyrene)), NameOf(WaterAnalysis.Pyrene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.BenzoAAnthracene)), NameOf(WaterAnalysis.BenzoAAnthracene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Crysene)), NameOf(WaterAnalysis.Crysene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.BenzoAPyrene)), NameOf(WaterAnalysis.BenzoAPyrene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.BenzoBFluoranthene)), NameOf(WaterAnalysis.BenzoBFluoranthene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.BenzoGHIPerylene)), NameOf(WaterAnalysis.BenzoGHIPerylene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.BenzoKFluoranthene)), NameOf(WaterAnalysis.BenzoKFluoranthene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.DibenzoAHAnthracene)), NameOf(WaterAnalysis.DibenzoAHAnthracene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Indeno123CDPyrene)), NameOf(WaterAnalysis.Indeno123CDPyrene)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Arsenic)), NameOf(WaterAnalysis.Arsenic)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Cadmium)), NameOf(WaterAnalysis.Cadmium)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Copper)), NameOf(WaterAnalysis.Copper)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Cobalt)), NameOf(WaterAnalysis.Cobalt)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.TotalChrome)), NameOf(WaterAnalysis.TotalChrome)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Mercury)), NameOf(WaterAnalysis.Mercury)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Nickel)), NameOf(WaterAnalysis.Nickel)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Lead)), NameOf(WaterAnalysis.Lead)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Selenium)), NameOf(WaterAnalysis.Selenium)},
        {GetDisplayName(Of WaterAnalysis)(NameOf(WaterAnalysis.Zinc)), NameOf(WaterAnalysis.Zinc)}}
End Class
