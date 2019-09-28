Imports Wells.YPFModel
Imports Wells.CorePersistence.Repositories
Imports Wells.ViewBase
Imports Wells.CorePersistence

Public Class MainWindowViewModel
    Inherits BaseViewModel

    Private _Window As IMainWindowView
    Private _Repository As RepositoryWrapper
    Private _RepositoryIsOpened As Boolean


    ReadOnly Property RepositoryIsOpened As Boolean
        Get
            Return _RepositoryIsOpened
        End Get
    End Property


    Sub New(window As IView)
        MyBase.New(window)
        _window = CType(window, IMainWindowView)

        ChemicalAnalysis.CreateParamtersDictionary()
    End Sub

    Property CreateDatabaseCommand As ICommand = New RelayCommand(Sub()
                                                                      Dim databaseName As String = ""
                                                                      ' Dim databasePath As String = ""
                                                                      If _Window.CreateDatabaseDialog(databaseName) Then
                                                                          Dim filename = IO.Path.Combine(databaseName & ".mdf")
                                                                          My.Settings.DatabaseFilename = filename
                                                                          My.Settings.Save()
                                                                          InitializeContext(filename, True)
                                                                      End If
                                                                  End Sub, Function() True, AddressOf OnError)

    Property OpenDatabaseCommand As ICommand = New RelayCommand(Sub()
                                                                    Dim filename = _Window.OpenFileDialog("Well Databases|*.mdf", "Abrir base de datos")
                                                                    If Not String.IsNullOrEmpty(filename) Then
                                                                        My.Settings.DatabaseFilename = IO.Path.GetFileName(filename)
                                                                        My.Settings.Save()
                                                                        InitializeContext(My.Settings.DatabaseFilename, False)
                                                                    End If
                                                                End Sub, Function() True, AddressOf OnError)



    Property OpenGraphicsViewCommand As ICommand = New RelayCommand(Sub()
                                                                        _Window.OpenGraphicsView()
                                                                    End Sub,
                                                               Function()
                                                                   Return RepositoryWrapper.IsInstatiated
                                                               End Function, AddressOf OnError)



    Private Sub ExportRejectedToExcel(rejected As List(Of RejectedEntity))
        If _Window.ShowMessageBox($"No se pudieron importar {rejected.Count} registro(s). ¿Desea exportar estos datos a un nuevo archivo Excel?", "Datos rechazados") Then
            Dim filename = _Window.SaveFileDialog("Archivos de Excel|*.xlsx", "Datos rechazados")
            If Not String.IsNullOrEmpty(filename) Then
                ExcelReader.ExportRejectedToExcel(rejected, filename)
            End If
        End If
    End Sub

    Sub InitializeContext(databaseFile As String, create As Boolean)
        Try
            _Window.ShowWaitingMessage("Abriendo la base de datos")
            Dim conString = $"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={databaseFile};Integrated Security=True"
            RepositoryWrapper.Instantiate(conString)
            _Repository = RepositoryWrapper.Instance
            _RepositoryIsOpened = _Repository IsNot Nothing
        Catch ex As Exception
            OnError(ex)
        Finally
            NotifyPropertyChanged(NameOf(RepositoryIsOpened))
        End Try
    End Sub



    Protected Overrides Sub SetValidators()
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub SetCommandUpdates()
        Throw New NotImplementedException()
    End Sub



End Class

