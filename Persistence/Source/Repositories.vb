Imports System.Data.Entity
Imports Wells.Model

Public Class Repositories

    Private _context As Context
    Private _fileManager As ExternalFileManager
    Private _databasePath As String

    Property Wells As WellsRepository
    Property ChemicalAnalysis As ChemicalAnalysisRepository
    Property Measurements As MeasurementsRepository
    Property Precipitations As PrecipitationsRepository
    Property Links As ExternalLinksRepository

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
        Wells = New WellsRepository(_context, Me, _context.Wells)
        ChemicalAnalysis = New ChemicalAnalysisRepository(_context, Me, _context.ChemicalAnalysis)
        Measurements = New MeasurementsRepository(_context, Me, _context.Measurements)
        Precipitations = New PrecipitationsRepository(_context, Me, _context.Precipitations)
        Links = New ExternalLinksRepository(_context, Me, _context.Links)
    End Sub

    Function RepositoryOf(type As Type) As IRepository(Of IBusinessObject)
        If type = GetType(Well) Then
            Return Wells
        ElseIf type = GetType(Measurement) Then
            Return Measurements
        ElseIf type = GetType(ChemicalAnalysis) Then
            Return ChemicalAnalysis
        ElseIf type = GetType(Precipitation) Then
            Return Precipitations
        ElseIf type = GetType(ExternalLink) Then
            Return Links
        End If
        Return Nothing
    End Function

    Sub Close()
        _context?.Close()
    End Sub

    Sub SaveChanges()
        _context.SaveChanges()
    End Sub
End Class
