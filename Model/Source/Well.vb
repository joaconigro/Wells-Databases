﻿Imports System.ComponentModel
Imports Wells.Model

Public Class Well
    Inherits BusinessObject
    Implements IComparable(Of Well)

    <DisplayName("Nombre"), Browsable(True)>
    Property Name As String

    <DisplayName("X"), Browsable(True)>
    Property X As Double

    <DisplayName("Y"), Browsable(True)>
    Property Y As Double

    <DisplayName("Cota"), Browsable(True)>
    Property Z As Double

    <DisplayName("Latitud"), Browsable(True)>
    Property Latitude As Double

    <DisplayName("Longitud"), Browsable(True)>
    Property Longitude As Double

    <Browsable(False)>
    ReadOnly Property HasGeographic As Boolean
        Get
            Return Not (Latitude = NullNumericValue AndAlso Longitude = NullNumericValue)
        End Get
    End Property

    <Browsable(False)>
    ReadOnly Property HasHeight As Boolean
        Get
            Return Height <> NullNumericValue
        End Get
    End Property

    <Browsable(False)>
    Property Type As WellType

    <DisplayName("Altura"), Browsable(True)>
    Property Height As Double

    <DisplayName("Existe"), Browsable(True)>
    Property Exists As Boolean

    <DisplayName("Fondo"), Browsable(True)>
    Property Bottom As Double

    <Browsable(False)>
    Property Analysis As New List(Of ChemicalAnalysis)

    <Browsable(False)>
    Property Measurements As New List(Of Measurement)

    <Browsable(False)>
    Property Links As New List(Of ExternalLink)

    <Browsable(False)>
    ReadOnly Property SoilAnalysis As List(Of ChemicalAnalysis)
        Get
            Dim list = (From a In Analysis
                        Where a.SampleOf = SampleType.Soil
                        Select a).ToList
            Return list
        End Get
    End Property

    <Browsable(False)>
    ReadOnly Property WaterAnalysis As List(Of ChemicalAnalysis)
        Get
            Dim list = (From a In Analysis
                        Where a.SampleOf = SampleType.Water
                        Select a).ToList
            Return list
        End Get
    End Property

    <Browsable(False)>
    ReadOnly Property FLNAAnalysis As List(Of ChemicalAnalysis)
        Get
            Dim list = (From a In Analysis
                        Where a.SampleOf = SampleType.FLNA
                        Select a).ToList
            Return list
        End Get
    End Property

    Sub New()
        MyBase.New()
    End Sub

    Sub New(name As String)
        MyBase.New()
        Me.Name = name
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Function CompareTo(other As Well) As Integer Implements IComparable(Of Well).CompareTo
        If Name > other.Name Then Return -1
        If Name = other.Name Then Return 0
        Return 1
    End Function
End Class

Public Enum WellType
    MeasurementWell
    Sounding
End Enum
