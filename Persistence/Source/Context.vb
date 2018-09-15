Imports System.Data.Entity
Imports Wells.Model

Public Class Context
    Inherits DbContext

    Property Wells As DbSet(Of Well)
    Property ChemicalAnalysis As DbSet(Of ChemicalAnalysis)
    Property Measurements As DbSet(Of Measurement)
    Property Precipitations As DbSet(Of Precipitation)
    Property Links As DbSet(Of ExternalLink)

    Sub New()
        MyBase.New()
    End Sub

    Sub New(connectionString As String, create As Boolean, initializator As IDatabaseInitializer(Of Context))
        MyBase.New(connectionString)
    End Sub

    Sub Close()
        SaveChanges()
        Dispose()
    End Sub
End Class
