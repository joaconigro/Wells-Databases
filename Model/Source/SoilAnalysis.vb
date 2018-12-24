Imports System.ComponentModel
Imports Wells.Model.ReflectionExtension

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

    <DisplayName("Hidrocarburos totales (EPA 8015)"), Browsable(True)>
    Property TotalHydrocarbons_EPA8015 As Double

    <DisplayName("Hidrocarburos totales (TNRCC 1005)"), Browsable(True)>
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

    <DisplayName("> C6 - C8 (F. aromática)"), Browsable(True)>
    Property C6_C8Aromatic As Double

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

    <DisplayName("Selenio"), Browsable(True)>
    Property Selenium As Double

    <DisplayName("Zinc"), Browsable(True)>
    Property Zinc As Double

    Public Overrides Function GetChemicalAnalysisType(propertyName As String) As ChemicalAnalysisType
        Return SoilAnalysisTypes(propertyName)
    End Function

    Shared ReadOnly Property Properties As New Dictionary(Of String, String) From {
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Humidity)), NameOf(SoilAnalysis.Humidity)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.PH)), NameOf(SoilAnalysis.PH)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.DRO)), NameOf(SoilAnalysis.DRO)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.GRO)), NameOf(SoilAnalysis.GRO)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.MRO)), NameOf(SoilAnalysis.MRO)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.TotalHydrocarbons_EPA8015)), NameOf(SoilAnalysis.TotalHydrocarbons_EPA8015)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.TotalHydrocarbons_TNRCC1005)), NameOf(SoilAnalysis.TotalHydrocarbons_TNRCC1005)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.OilsAndFats)), NameOf(SoilAnalysis.OilsAndFats)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C6_C8Aliphatic)), NameOf(SoilAnalysis.C6_C8Aliphatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C8_C10Aliphatic)), NameOf(SoilAnalysis.C8_C10Aliphatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C10_C12Aliphatic)), NameOf(SoilAnalysis.C10_C12Aliphatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C12_C16Aliphatic)), NameOf(SoilAnalysis.C12_C16Aliphatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C16_C21Aliphatic)), NameOf(SoilAnalysis.C16_C21Aliphatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C21_C35Aliphatic)), NameOf(SoilAnalysis.C21_C35Aliphatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C6_C8Aromatic)), NameOf(SoilAnalysis.C6_C8Aromatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C8_C10Aromatic)), NameOf(SoilAnalysis.C8_C10Aromatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C10_C12Aromatic)), NameOf(SoilAnalysis.C10_C12Aromatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C12_C16Aromatic)), NameOf(SoilAnalysis.C12_C16Aromatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C16_C21Aromatic)), NameOf(SoilAnalysis.C16_C21Aromatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.C21_C35Aromatic)), NameOf(SoilAnalysis.C21_C35Aromatic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Benzene)), NameOf(SoilAnalysis.Benzene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Tolueno)), NameOf(SoilAnalysis.Tolueno)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Ethylbenzene)), NameOf(SoilAnalysis.Ethylbenzene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.XyleneO)), NameOf(SoilAnalysis.XyleneO)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.XylenePM)), NameOf(SoilAnalysis.XylenePM)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.TotalXylene)), NameOf(SoilAnalysis.TotalXylene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Naphthalene)), NameOf(SoilAnalysis.Naphthalene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Acenafthene)), NameOf(SoilAnalysis.Acenafthene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Acenaphthylene)), NameOf(SoilAnalysis.Acenaphthylene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Fluorene)), NameOf(SoilAnalysis.Fluorene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Anthracene)), NameOf(SoilAnalysis.Anthracene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Fenanthrene)), NameOf(SoilAnalysis.Fenanthrene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Fluoranthene)), NameOf(SoilAnalysis.Fluoranthene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Pyrene)), NameOf(SoilAnalysis.Pyrene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Crysene)), NameOf(SoilAnalysis.Crysene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.BenzoAAnthracene)), NameOf(SoilAnalysis.BenzoAAnthracene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.BenzoAPyrene)), NameOf(SoilAnalysis.BenzoAPyrene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.BenzoBFluoranthene)), NameOf(SoilAnalysis.BenzoBFluoranthene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.BenzoGHIPerylene)), NameOf(SoilAnalysis.BenzoGHIPerylene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.BenzoKFluoranthene)), NameOf(SoilAnalysis.BenzoKFluoranthene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.DibenzoAHAnthracene)), NameOf(SoilAnalysis.DibenzoAHAnthracene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Indeno123CDPyrene)), NameOf(SoilAnalysis.Indeno123CDPyrene)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Arsenic)), NameOf(SoilAnalysis.Arsenic)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Cadmium)), NameOf(SoilAnalysis.Cadmium)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Copper)), NameOf(SoilAnalysis.Copper)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.TotalChrome)), NameOf(SoilAnalysis.TotalChrome)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Mercury)), NameOf(SoilAnalysis.Mercury)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Nickel)), NameOf(SoilAnalysis.Nickel)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Lead)), NameOf(SoilAnalysis.Lead)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Zinc)), NameOf(SoilAnalysis.Zinc)},
       {GetDisplayName(Of SoilAnalysis)(NameOf(SoilAnalysis.Selenium)), NameOf(SoilAnalysis.Selenium)}}
End Class
