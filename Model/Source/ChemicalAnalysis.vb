Imports System.ComponentModel
Imports System.IO

Public MustInherit Class ChemicalAnalysis
    Inherits BusinessObject
    Implements IComparable(Of ChemicalAnalysis)

    <Browsable(False)>
    ReadOnly Property WellId As String
        Get
            Return Well?.Id
        End Get
    End Property

    <Browsable(False)>
    Property SampleDate As String

    <Browsable(False)>
    Property SampleOf As SampleType

    <Browsable(False)>
    Property Well As Well

    Protected _WellName As String
    MustOverride Property WellName As String

    MustOverride ReadOnly Property RealDate As Date

    Private Shared _FLNAAnalysisTypes As Dictionary(Of String, ChemicalAnalysisType)
    Private Shared _WaterAnalysisTypes As Dictionary(Of String, ChemicalAnalysisType)
    Private Shared _SoilAnalysisTypes As Dictionary(Of String, ChemicalAnalysisType)

    Shared ReadOnly Property FLNAAnalysisTypes As Dictionary(Of String, ChemicalAnalysisType)
        Get
            Return _FLNAAnalysisTypes
        End Get
    End Property

    Shared ReadOnly Property WaterAnalysisTypes As Dictionary(Of String, ChemicalAnalysisType)
        Get
            Return _WaterAnalysisTypes
        End Get
    End Property

    Shared ReadOnly Property SoilAnalysisTypes As Dictionary(Of String, ChemicalAnalysisType)
        Get
            Return _SoilAnalysisTypes
        End Get
    End Property

    Protected Sub New()
        MyBase.New()
    End Sub

    Protected Sub New(well As Well)
        MyBase.New()
        Me.Well = well
    End Sub

    Public Overrides Function ToString() As String
        Return SampleDate
    End Function

    Public Function CompareTo(other As ChemicalAnalysis) As Integer Implements IComparable(Of ChemicalAnalysis).CompareTo
        If RealDate > other.RealDate Then Return -1
        If RealDate = other.RealDate Then Return 0
        Return 1
    End Function

    Public MustOverride Function GetChemicalAnalysisType(propertyName As String) As ChemicalAnalysisType


    Shared Sub CreateParamtersDictionary()
        _FLNAAnalysisTypes = ReadParametersFromResource(My.Resources.FLNA)
        _WaterAnalysisTypes = ReadParametersFromResource(My.Resources.Water)
        _SoilAnalysisTypes = ReadParametersFromResource(My.Resources.Soil)
    End Sub

    Private Shared Function ReadParametersFromResource(resource As String) As Dictionary(Of String, ChemicalAnalysisType)
        Dim dict As New Dictionary(Of String, ChemicalAnalysisType)
        Using sr As New StringReader(resource)
            Dim line As String = sr.ReadLine
            While line <> Nothing
                Dim split = line.Trim.Split({Chr(9)}, StringSplitOptions.RemoveEmptyEntries)
                dict.Add(split(1), New ChemicalAnalysisType(split(1), split(0), split(3), split(2)))
                line = sr.ReadLine
            End While
        End Using
        Return dict
    End Function
End Class

Public Structure ChemicalAnalysisType
    Property Parameter As String
    Property Group As String
    Property Unit As String
    Property Technique As String

    Sub New(parameter As String, group As String, unit As String, technique As String)
        Me.Parameter = parameter
        Me.Group = group
        Me.Unit = unit
        Me.Technique = technique
    End Sub
End Structure

Public Enum SampleType
    Water
    FLNA
    Soil
End Enum
