Imports System.ComponentModel

Public Class Measurement
    Inherits BusinessObject

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

    <Browsable(False)>
    Property SampleDate As String

    <DisplayName("Profundidad FLNA"), Browsable(True)>
    Property FLNADepth As Double

    <DisplayName("Profundidad Agua"), Browsable(True)>
    Property WaterDepth As Double

    <DisplayName("Caudal"), Browsable(True)>
    Property Caudal As Double

    <DisplayName("Observaciones"), Browsable(True)>
    Property Comment As String

    <Browsable(False)>
    Property Well As Well

    <DisplayName("Fecha"), Browsable(True)>
    ReadOnly Property RealDate As Date
        Get
            Return Date.ParseExact(SampleDate, "dd/MM/yyyy", Nothing)
        End Get
    End Property

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

    <DisplayName("Espesor FLNA"), Browsable(True)>
    ReadOnly Property FLNAThickness As Double
        Get
            If HasFLNA Then
                Return WaterDepth - FLNADepth
            Else
                Return 0
            End If
        End Get
    End Property

    <DisplayName("Cota Agua"), Browsable(True)>
    ReadOnly Property WaterElevation As Double
        Get
            Return Well?.Z + Well?.Height - WaterDepth
        End Get
    End Property

    <DisplayName("Cota FLNA"), Browsable(True)>
    ReadOnly Property FLNAElevation As Double
        Get
            Return Well?.Z + Well?.Height - FLNADepth
        End Get
    End Property
End Class


