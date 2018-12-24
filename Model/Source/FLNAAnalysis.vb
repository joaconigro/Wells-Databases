Imports System.ComponentModel
Imports Wells.Model.ReflectionExtension

Public Class FLNAAnalysis
    Inherits ChemicalAnalysis

    Sub New()
        MyBase.New()
        SampleOf = SampleType.FLNA
    End Sub

    Sub New(well As Well)
        MyBase.New(well)
        SampleOf = SampleType.FLNA
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

    <DisplayName("GRO"), Browsable(True)>
    Property GRO As Double

    <DisplayName("DRO"), Browsable(True)>
    Property DRO As Double

    <DisplayName("MRO"), Browsable(True)>
    Property MRO As Double

    <DisplayName("Benceno"), Browsable(True)>
    Property Benzene As Double

    <DisplayName("Tolueno"), Browsable(True)>
    Property Tolueno As Double

    <DisplayName("Etilbenceno"), Browsable(True)>
    Property Ethylbenzene As Double

    <DisplayName("Xilenos"), Browsable(True)>
    Property Xylenes As Double

    <DisplayName("C6 - C8"), Browsable(True)>
    Property C6_C8 As Double

    <DisplayName("C8 - C10"), Browsable(True)>
    Property C8_C10 As Double

    <DisplayName("C10 - C12"), Browsable(True)>
    Property C10_C12 As Double

    <DisplayName("C12 - C16"), Browsable(True)>
    Property C12_C16 As Double

    <DisplayName("C16 - C21"), Browsable(True)>
    Property C16_C21 As Double

    <DisplayName("C21 - C35"), Browsable(True)>
    Property C21_C35 As Double

    <DisplayName("C17/Pristano"), Browsable(True)>
    Property C17_Pristano As Double

    <DisplayName("C18/Fitano"), Browsable(True)>
    Property C18_Fitano As Double

    <DisplayName("Densidad Real"), Browsable(True)>
    Property RealDensity As Double

    <DisplayName("Viscosidad Dinámica"), Browsable(True)>
    Property DynamicViscosity As Double

    Public Overrides Function GetChemicalAnalysisType(propertyName As String) As ChemicalAnalysisType
        Return FLNAAnalysisTypes(propertyName)
    End Function

    Shared ReadOnly Property Properties As New Dictionary(Of String, String) From {
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.GRO)), NameOf(FLNAAnalysis.GRO)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.DRO)), NameOf(FLNAAnalysis.DRO)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.MRO)), NameOf(FLNAAnalysis.MRO)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.Benzene)), NameOf(FLNAAnalysis.Benzene)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.Tolueno)), NameOf(FLNAAnalysis.Tolueno)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.Ethylbenzene)), NameOf(FLNAAnalysis.Ethylbenzene)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.Xylenes)), NameOf(FLNAAnalysis.Xylenes)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C6_C8)), NameOf(FLNAAnalysis.C6_C8)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C8_C10)), NameOf(FLNAAnalysis.C8_C10)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C10_C12)), NameOf(FLNAAnalysis.C10_C12)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C12_C16)), NameOf(FLNAAnalysis.C12_C16)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C16_C21)), NameOf(FLNAAnalysis.C16_C21)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C21_C35)), NameOf(FLNAAnalysis.C21_C35)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C17_Pristano)), NameOf(FLNAAnalysis.C17_Pristano)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.C18_Fitano)), NameOf(FLNAAnalysis.C18_Fitano)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.RealDensity)), NameOf(FLNAAnalysis.RealDensity)},
       {GetDisplayName(Of FLNAAnalysis)(NameOf(FLNAAnalysis.DynamicViscosity)), NameOf(FLNAAnalysis.DynamicViscosity)}}
End Class
