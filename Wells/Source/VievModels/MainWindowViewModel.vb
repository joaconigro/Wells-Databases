Imports Wells.Model
Imports Wells.Persistence

Public Class MainWindowViewModel
    Inherits BaseViewModel

    Private _window As IMainWindowView
    Private repo As Repository

    Sub New(window As IMainWindowView)
        _window = window

        If Not String.IsNullOrEmpty(My.Settings.DatabaseFilename) AndAlso IO.File.Exists(My.Settings.DatabaseFilename) Then
            OpenDatabase(My.Settings.DatabaseFilename, False)
        End If
    End Sub

    Property Datasource As IEnumerable(Of IBusinessObject)


    Property CreateDatabaseCommand As ICommand = New Command(Sub()
                                                                 Dim databaseName As String = ""
                                                                 Dim databasePath As String = ""
                                                                 If _window.CreateDatabaseDialog(databaseName, databasePath) Then
                                                                     Dim filename = IO.Path.Combine(databasePath, databaseName & ".mdf")
                                                                     My.Settings.DatabaseFilename = filename
                                                                     My.Settings.Save()
                                                                     OpenDatabase(filename, True)
                                                                 End If
                                                             End Sub)

    Property OpenDatabaseCommand As ICommand = New Command(Sub()
                                                               Dim filename = _window.OpenFileDialog("Well Databases|*.mdf", "Abrir base de datos")
                                                               If Not String.IsNullOrEmpty(filename) Then
                                                                   My.Settings.DatabaseFilename = filename
                                                                   My.Settings.Save()
                                                                   OpenDatabase(filename, False)
                                                               End If
                                                           End Sub)

    Private Sub OpenDatabase(databaseFile As String, create As Boolean)
        repo?.Close()
        repo = New Repository(databaseFile, create)
        Datasource = repo.Measurements.Values.ToList
    End Sub

End Class
