Imports System.ComponentModel
Imports Wells.Model.ReflectionExtension

Public Class Precipitation
    Inherits BusinessObject

    <Browsable(False)>
    Property PrecipitationDate As String

    <DisplayName("Fecha"), Browsable(True)>
    ReadOnly Property RealDate As Date
        Get
            Return Date.ParseExact(PrecipitationDate, "dd/MM/yyyy", Nothing)
        End Get
    End Property

    <DisplayName("Milímetros"), Browsable(True)>
    Property Millimeters As Double

    Shared ReadOnly Property Propeties As New Dictionary(Of String, String) From {
       {GetDisplayName(Of Precipitation)(NameOf(Precipitation.Millimeters)), NameOf(Precipitation.Millimeters)}}
End Class
