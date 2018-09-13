Imports Wells.Model
Imports System.Data.Entity


Public Class Repository
    Inherits DbContext

    Property Wells As DbSet(Of Well)
    Property ChemicalAnalysis As DbSet(Of ChemicalAnalysis)
    Property Measurements As DbSet(Of Measurement)
    Property Precipitations As DbSet(Of Precipitation)
End Class
