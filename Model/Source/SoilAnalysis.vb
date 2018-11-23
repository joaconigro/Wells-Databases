Imports System.ComponentModel

Public Class SoilAnalysis
    Inherits ChemicalAnalysis

    Sub New()
        MyBase.New()
        SampleOf = SampleType.Soil
    End Sub

    Sub New(well As Well)
        MyBase.New(well)
        SampleOf = SampleType.Soil
    End Sub

    <DisplayName("Humedad"), Browsable(True)>
    Property Humidity As Double

    <DisplayName("pH"), Browsable(True)>
    Property PH As Double

    <DisplayName("GRO"), Browsable(True)>
    Property GRO As Double

    <DisplayName("DRO"), Browsable(True)>
    Property DRO As Double

    <DisplayName("MRO"), Browsable(True)>
    Property MRO As Double

    <DisplayName("Hidrocarburos totales(EPA 8015)"), Browsable(True)>
    Property TotalHydrocarbons_EPA8015 As Double

    <DisplayName("Hidrocarburos totales(TNRCC 1005)"), Browsable(True)>
    Property TotalHydrocarbons_TNRCC1005 As Double

    <DisplayName("Aceites y grasas"), Browsable(True)>
    Property OilsAndFats As Double

    <DisplayName("> C6 - C8 (F. alifática)"), Browsable(True)>
    Property C6_C8Aliphatic As Double

    <DisplayName("> C8 - C10 (F. alifática)"), Browsable(True)>
    Property C8_C10Aliphatic As Double

    <DisplayName("> C10 - C12 (F. alifática)"), Browsable(True)>
    Property C10_C12Aliphatic As Double

    <DisplayName("> C12 - C16 (F. alifática)"), Browsable(True)>
    Property C12_C16Aliphatic As Double

    <DisplayName("> C16 - C21 (F. alifática)"), Browsable(True)>
    Property C16_C21Aliphatic As Double

    <DisplayName("> C21 - C35 (F. alifática)"), Browsable(True)>
    Property C21_C35Aliphatic As Double

    <DisplayName("> C7 - C8 (F. aromática)"), Browsable(True)>
    Property C7_C8Aromatic As Double

    <DisplayName("> C8 - C10 (F. aromática)"), Browsable(True)>
    Property C8_C10Aromatic As Double

    <DisplayName("> C10 - C12 (F. aromática)"), Browsable(True)>
    Property C10_C12Aromatic As Double

    <DisplayName("> C12 - C16 (F. aromática)"), Browsable(True)>
    Property C12_C16Aromatic As Double

    <DisplayName("> C16 - C21 (F. aromática)"), Browsable(True)>
    Property C16_C21Aromatic As Double

    <DisplayName("> C21 - C35 (F. aromática)"), Browsable(True)>
    Property C21_C35Aromatic As Double

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
        Return SoilAnalysisTypes(propertyName)
    End Function
End Class
