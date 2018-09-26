Imports System.ComponentModel

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
End Class
