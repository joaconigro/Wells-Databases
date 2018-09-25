Imports System.Data.Entity
Imports Wells.Model

Public Class Context
    Inherits DbContext

    Property Wells As DbSet(Of Well)
    Property ChemicalAnalysis As DbSet(Of ChemicalAnalysis)
    Property Measurements As DbSet(Of Measurement)
    Property Precipitations As DbSet(Of Precipitation)
    Property Links As DbSet(Of ExternalLink)

    Private _databaseName As String

    Sub New()
        MyBase.New()
    End Sub

    Sub New(connectionString As String, create As Boolean, initializator As IDatabaseInitializer(Of Context))
        MyBase.New(connectionString)
        _databaseName = Database.Connection.Database
    End Sub

    Sub Add(entity As IBusinessObject)
        Dim entityType = entity.GetType
        MyBase.Set(entityType).Add(entity)
    End Sub

    Sub AddRange(entities As IEnumerable(Of IBusinessObject))
        Dim entityType = entities.First.GetType
        MyBase.Set(entityType).AddRange(entities)
    End Sub

    Sub Update(entity As IBusinessObject)
        MyBase.Entry(entity).State = EntityState.Modified
    End Sub

    Sub Delete(entity As IBusinessObject)
        Dim entityType = entity.GetType
        MyBase.Set(entityType).Remove(entity)
    End Sub

End Class
