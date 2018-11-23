Imports System.ComponentModel

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
End Class
