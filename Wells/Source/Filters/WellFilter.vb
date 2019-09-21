Imports System.ComponentModel
Imports Wells.CorePersistence.Repositories
Imports Wells.Base.Common
Imports Wells.StandardModel.Models
Imports Wells.YPFModel

Public Class WellFilter(Of T)
    Inherits BaseFilter(Of T)

    ReadOnly Property QueryProperty As WellQueryProperty
    ReadOnly Property WellType As WellTypes
    ReadOnly Property WellName As String

    Private _ApplyToWellsOnly As Boolean

    Public Overrides ReadOnly Property Description As String
        Get
            Return CreateDescription()
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayValue As String
        Get
            Return WellName
        End Get
    End Property

    Public Overrides ReadOnly Property IsDateRangeFilter As Boolean
        Get
            Return False
        End Get
    End Property

    Sub New(repo As IBussinessObjectRepository, applyToWellsOnly As Boolean, wellType As Integer, wellProperty As Integer, wellName As String)
        MyBase.New("Well", String.Empty, repo)
        _IsEditable = False
        Me.WellType = wellType
        QueryProperty = wellProperty
        Me.WellName = wellName
        _ApplyToWellsOnly = applyToWellsOnly
    End Sub

    Public Overrides Function Apply(originalList As IQueryable(Of T)) As IQueryable(Of T)
        If _ApplyToWellsOnly Then
            Return From o In originalList
                   Let w = CType(CType(o, Object), Well)
                   Where IsThisWell(w)
                   Select o
        Else
            Return From o In originalList
                   Let w = CType(CallByName(o, PropertyName, CallType.Get), Well)
                   Where IsThisWell(w)
                   Select o
        End If

    End Function

    Private Function IsThisWell(well As Well) As Boolean
        Dim result = FilterByWellType(well)
        If Not result Then
            Return False
        End If
        Return FilterByWellProperty(well)
    End Function

    Private Function FilterByWellType(well As Well) As Boolean
        Select Case WellType
            Case WellTypes.OnlyMeasurementWell
                Return well.WellType = StandardModel.Models.WellType.MeasurementWell
            Case WellTypes.OnlySounding
                Return well.WellType = StandardModel.Models.WellType.Sounding
            Case Else
                Return True
        End Select
    End Function

    Private Function FilterByWellProperty(well As Well) As Boolean
        Select Case QueryProperty
            Case WellQueryProperty.Name
                If String.IsNullOrEmpty(WellName) Then
                    Return False
                Else
                    Return well.Name = WellName
                End If
            Case WellQueryProperty.ZoneA
                Return Rectangle2D.ZoneA.Contains(well)
            Case WellQueryProperty.ZoneB
                Return Rectangle2D.ZoneB.Contains(well)
            Case WellQueryProperty.ZoneC
                Return Rectangle2D.ZoneC.Contains(well)
            Case WellQueryProperty.ZoneD
                Return Rectangle2D.ZoneD.Contains(well)
            Case WellQueryProperty.Torches
                Return Rectangle2D.Torches.Contains(well)
            Case Else
                Return True
        End Select
    End Function

    Private Function CreateDescription() As String
        Dim text As String
        Select Case WellType
            Case WellTypes.OnlyMeasurementWell
                text = "Todos los pozos "
            Case WellTypes.OnlySounding
                text = "Todos los sondeos "
            Case Else
                text = "Todos los pozos y sondeos "
        End Select

        Select Case QueryProperty
            Case WellQueryProperty.Name
                text &= $"con nombre = {WellName}"
            Case WellQueryProperty.ZoneA
                text &= "dentro de la zona A"
            Case WellQueryProperty.ZoneB
                text &= "dentro de la zona B"
            Case WellQueryProperty.ZoneC
                text &= "dentro de la zona C"
            Case WellQueryProperty.ZoneD
                text &= "dentro de la zona D"
            Case WellQueryProperty.Torches
                text &= "dentro de la zona Antorchas"
            Case Else
                text = text.Trim
        End Select

        Return text
    End Function
End Class

Public Enum WellTypes
    <Description("Todos")> All
    <Description("Pozo")> OnlyMeasurementWell
    <Description("Sondeo")> OnlySounding
End Enum

Public Enum WellQueryProperty
    <Description("Ninguna")> None
    <Description("Nombre")> Name
    <Description("Zona A")> ZoneA
    <Description("Zona B")> ZoneB
    <Description("Zona C")> ZoneC
    <Description("Zona D")> ZoneD
    <Description("Zona Antorchas")> Torches
End Enum