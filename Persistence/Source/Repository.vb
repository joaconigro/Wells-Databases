Imports Wells.Model
Imports System.Data.Entity

Public Class Repository
    Implements IRepository

    Private _context As Context
    Private _fileManager As ExternalFileManager
    Private _databasePath As String

    Property Wells As Dictionary(Of String, Well)
    Property WellNames As Dictionary(Of String, Well)
    Property ChemicalAnalysis As Dictionary(Of String, ChemicalAnalysis)
    Property Measurements As Dictionary(Of String, Measurement)
    Property Precipitations As Dictionary(Of String, Precipitation)
    Property Links As Dictionary(Of String, ExternalLink)

    Sub New(filename As String, create As Boolean)
        _fileManager = New ExternalFileManager(IO.Path.GetDirectoryName(filename))
        _databasePath = filename
        Dim conString = $"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={filename};Integrated Security=True"
        Dim initializator As IDatabaseInitializer(Of Context)
        If create Then
            initializator = New CreateDatabaseIfNotExists(Of Context)
        Else
            initializator = New MigrateDatabaseToLatestVersion(Of Context, Migrations.Configuration)(True)
        End If
        Database.SetInitializer(initializator)
        _context = New Context(conString, create, initializator)
        initializator.InitializeDatabase(_context)
        Initialize()
    End Sub

    Private Sub Initialize()
        Wells = _context.Wells.ToDictionary(Function(e) e.Id)
        WellNames = _context.Wells.ToDictionary(Function(e) e.Name)
        ChemicalAnalysis = _context.ChemicalAnalysis.ToDictionary(Function(e) e.Id)
        Measurements = _context.Measurements.ToDictionary(Function(e) e.Id)
        Precipitations = _context.Precipitations.ToDictionary(Function(e) e.Id)
        Links = _context.Links.ToDictionary(Function(e) e.Id)
    End Sub

    Sub Close()
        _context?.Close()
    End Sub

    Public Sub Add(entity As IBusinessObject) Implements IRepository.Add
        Dim entityType = entity.GetType
        _context.Set(entityType).Add(entity)
    End Sub

    Public Sub Update(entity As IBusinessObject) Implements IRepository.Update
        _context.Entry(entity).State = EntityState.Modified
    End Sub

    Public Sub Delete(entity As IBusinessObject) Implements IRepository.Delete
        Dim entityType = entity.GetType
        _context.Set(entityType).Remove(entity)
    End Sub

    Sub SaveChanges()
        _context.SaveChanges()
    End Sub

    Function Add(well As Well) As Boolean
        If Wells.ContainsKey(well.Id) OrElse WellNames.ContainsKey(well.Name) Then
            Return False
        Else
            Wells.Add(well.Id, well)
            WellNames.Add(well.Name, well)
            Add(CType(well, IBusinessObject))
            Return True
        End If
    End Function

    Function AddRange(wells As IEnumerable(Of Well)) As List(Of Well)
        Dim rejecteds As New List(Of Well)
        For Each w In wells
            If Not Add(w) Then
                rejecteds.Add(w)
            End If
        Next
        Return rejecteds
    End Function

    Function FindWell(id As String) As Well
        If Wells.ContainsKey(id) Then
            Return Wells(id)
        End If
        Return Nothing
    End Function

    Function FindWellName(name As String) As Well
        If WellNames.ContainsKey(name) Then
            Return WellNames(name)
        End If
        Return Nothing
    End Function

    Function WellExists(id As String) As Boolean
        Return Wells.ContainsKey(id)
    End Function

    Function Add(measurement As Measurement) As Boolean
        If Not WellNames.ContainsKey(measurement.WellName) OrElse Measurements.ContainsKey(measurement.Id) Then
            Return False
        Else
            Measurements.Add(measurement.Id, measurement)
            Dim well = WellNames(measurement.WellName)
            well.Measures.Add(measurement)
            measurement.Well = well
            Add(CType(measurement, IBusinessObject))
            Return True
        End If
    End Function

    Function AddRange(measurements As IEnumerable(Of Measurement)) As List(Of Measurement)
        Dim rejecteds As New List(Of Measurement)
        For Each m In measurements
            Dim i = measurements.ToList.IndexOf(m)
            If Not Add(m) Then
                rejecteds.Add(m)
            End If
            Exit For
        Next
        Return rejecteds
    End Function


    Function FindChemicalAnalysis(id As String) As ChemicalAnalysis
        If ChemicalAnalysis.ContainsKey(id) Then
            Return ChemicalAnalysis(id)
        End If
        Return Nothing
    End Function

    Function ChemicalAnalysisExists(id As String) As Boolean
        Return ChemicalAnalysis.ContainsKey(id)
    End Function
End Class
