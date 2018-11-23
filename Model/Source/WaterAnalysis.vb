Imports System.ComponentModel

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

    <DisplayName("Sulfuros Totales(HS -)"), Browsable(True)>
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

    <DisplayName("Hidrocarburos totales(EPA 8015)"), Browsable(True)>
    Property TotalHydrocarbons_EPA8015 As Double

    <DisplayName("Hidrocarburos totales(TNRCC 1005)"), Browsable(True)>
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

    <DisplayName("Criseno"), Browsable(True)>
    Property Crysene As Double

    <DisplayName("Benzo(a)antraceno"), Browsable(True)>
    Property BenzoAAnthracene As Double

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

    <DisplayName("Zinc"), Browsable(True)>
    Property Zinc As Double

    <DisplayName("Selenio"), Browsable(True)>
    Property Selenium As Double

    Public Overrides Function GetChemicalAnalysisType(propertyName As String) As ChemicalAnalysisType
        Return WaterAnalysisTypes(propertyName)
    End Function
End Class
