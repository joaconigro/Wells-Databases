Imports Wells.Model
Imports System.Data.Entity


Public Class Repository
    Inherits DbContext

    Property WellsSet As DbSet(Of Well)
    Property ChemicalAnalysisSet As DbSet(Of ChemicalAnalysis)
    Property MeasurementsSet As DbSet(Of Measurement)
    Property PrecipitationsSet As DbSet(Of Precipitation)
    Property LinksSet As DbSet(Of ExternalLink)

    Property Wells As List(Of Well)
    Property ChemicalAnalysis As List(Of ChemicalAnalysis)
    Property Measurements As List(Of Measurement)
    Property Precipitations As List(Of Precipitation)
    Property Links As List(Of ExternalLink)

    Sub New()
        Initialize()

    End Sub

    Sub New(nameOrConnectionString As String)
        MyBase.New(nameOrConnectionString)
        Dim initializator = New CreateDatabaseIfNotExists(Of Repository)
        Database.SetInitializer(Of Repository)(initializator)
        initializator.InitializeDatabase(Me)
        Initialize()
    End Sub

    Private Sub Initialize()
        Wells = WellsSet.ToList
        ChemicalAnalysis = ChemicalAnalysisSet.ToList
        Measurements = MeasurementsSet.ToList
        Precipitations = PrecipitationsSet.ToList
        Links = LinksSet.ToList
    End Sub
End Class
