Imports Wells.Model
Imports Wells.Persistence

Public Class MainWindowViewModel
    Inherits BaseViewModel

    Private _window As IMainWindowView
    Private repo As Repository

    Sub New(window As IMainWindowView)
        _window = window

        If Not String.IsNullOrEmpty(My.Settings.DatabaseFilename) AndAlso IO.File.Exists(My.Settings.DatabaseFilename) Then
            repo = New Repository(My.Settings.DatabaseFilename, False)
            Datasource = repo.Wells.Values.ToList
        End If
    End Sub

    Property Datasource As IEnumerable(Of IBusinessObject)


    Property CreateDatabaseCommand As ICommand = New Command(Sub()
                                                                 Dim filename = _window.SaveFileDialog("Well Databases|*.mdf", "Nueva base de datos")
                                                                 If Not String.IsNullOrEmpty(filename) Then
                                                                     My.Settings.DatabaseFilename = filename
                                                                     My.Settings.Save()
                                                                     repo = New Repository(filename, True)
                                                                     Datasource = repo.Wells.Values.ToList
                                                                 End If
                                                             End Sub)

    Property OpenDatabaseCommand As ICommand = New Command(Sub()
                                                               Dim filename = _window.OpenFileDialog("Well Databases|*.mdf", "Abrir base de datos")
                                                               If Not String.IsNullOrEmpty(filename) Then
                                                                   My.Settings.DatabaseFilename = filename
                                                                   My.Settings.Save()
                                                                   repo = New Repository(filename, True)
                                                                   Datasource = repo.Wells.Values.ToList
                                                               End If
                                                           End Sub)

End Class
