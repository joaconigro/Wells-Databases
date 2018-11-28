Imports System.ComponentModel
Imports Wells.Model.ReflectionExtension

Public Class Measurement
    Inherits BusinessObject
    Implements IComparable(Of Measurement)

    <Browsable(False)>
    ReadOnly Property WellId As String
        Get
            Return Well?.Id
        End Get
    End Property

    Private _WellName As String
    <DisplayName("Pozo"), Browsable(True)>
    Property WellName As String
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
    ReadOnly Property RealDate As Date
        Get
            Return Date.ParseExact(SampleDate, "dd/MM/yyyy", Nothing)
        End Get
    End Property

    <Browsable(False)>
    Property SampleDate As String

    <DisplayName("Profundidad FLNA"), Browsable(True)>
    Property FLNADepth As Double

    <DisplayName("Profundidad Agua"), Browsable(True)>
    Property WaterDepth As Double

    <DisplayName("Espesor FLNA"), Browsable(True)>
    ReadOnly Property FLNAThickness As Double
        Get
            If HasFLNA AndAlso HasWater Then
                Return WaterDepth - FLNADepth
            Else
                Return NullNumericValue
            End If
        End Get
    End Property

    <DisplayName("Cota FLNA"), Browsable(True)>
    ReadOnly Property FLNAElevation As Double
        Get
            If HasFLNA Then
                If Well?.HasHeight Then
                    Return Well?.Z + Well?.Height - FLNADepth
                Else
                    Return Well?.Z - FLNADepth
                End If
            End If
            Return NullNumericValue
        End Get
    End Property

    <DisplayName("Cota Agua"), Browsable(True)>
    ReadOnly Property WaterElevation As Double
        Get
            If HasWater Then
                If Well?.HasHeight Then
                    Return Well?.Z + Well?.Height - WaterDepth
                Else
                    Return Well?.Z - WaterDepth
                End If
            End If
            Return NullNumericValue
        End Get
    End Property

    <DisplayName("Caudal"), Browsable(True)>
    Property Caudal As Double

    <DisplayName("Observaciones"), Browsable(True)>
    Property Comment As String

    <Browsable(False)>
    Property Well As Well

    <Browsable(False)>
    ReadOnly Property HasFLNA As Boolean
        Get
            Return FLNADepth <> NullNumericValue
        End Get
    End Property

    <Browsable(False)>
    ReadOnly Property HasWater As Boolean
        Get
            Return WaterDepth <> NullNumericValue
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return SampleDate
    End Function

    Public Function CompareTo(other As Measurement) As Integer Implements IComparable(Of Measurement).CompareTo
        If RealDate > other.RealDate Then Return -1
        If RealDate = other.RealDate Then Return 0
        Return 1
    End Function

    Shared ReadOnly Property Propeties As New Dictionary(Of String, String) From {
        {GetDisplayName(Of Measurement)(NameOf(Measurement.FLNADepth)), NameOf(Measurement.FLNADepth)},
        {GetDisplayName(Of Measurement)(NameOf(Measurement.WaterDepth)), NameOf(Measurement.WaterDepth)},
        {GetDisplayName(Of Measurement)(NameOf(Measurement.Caudal)), NameOf(Measurement.Caudal)},
        {GetDisplayName(Of Measurement)(NameOf(Measurement.FLNAThickness)), NameOf(Measurement.FLNAThickness)},
        {GetDisplayName(Of Measurement)(NameOf(Measurement.WaterElevation)), NameOf(Measurement.WaterElevation)},
        {GetDisplayName(Of Measurement)(NameOf(Measurement.FLNAElevation)), NameOf(Measurement.FLNAElevation)}}
End Class


