Imports Wells.YPFModel

Imports Wells.CorePersistence
Imports Wells.ViewBase
Imports Wells.CorePersistence.Repositories

Public Class EditWellViewModel
    Inherits BaseViewModel

    Property WellName As String
        Get
            Return _WellName
        End Get
        Set
            _WellName = Value
            CType(AcceptCommand, RelayCommand).RaiseCanExecuteChanged()
            CType(EditMeasurementCommand, RelayCommand).RaiseCanExecuteChanged()
            CType(NewMeasurementCommand, RelayCommand).RaiseCanExecuteChanged()
            CType(NewExternalLinkCommand, RelayCommand).RaiseCanExecuteChanged()
        End Set
    End Property

    Property X As Double
    Property Y As Double
    Property Z As Double
    Property Latitude As Double
    Property Longitude As Double
    Property Type As Integer
    Property Height As Double
    Property Exists As Boolean
    Property Bottom As Double
    Property Analysis As New List(Of ChemicalAnalysis)
    Property Measurements As New List(Of Measurement)
    Property Links As New List(Of ExternalFile)

    ReadOnly Property HasWell As Boolean
    ReadOnly Property NameEditable As Boolean
    ReadOnly Property WellTypes As New List(Of String) From {"Pozo", "Sondeo"}

    Private _well As Well
    Private _repo As WellsRepository
    Private _measurementEditViewModel As EditMeasurementViewModel
    Private _WellName As String
    Private _View As IView

    Event CloseDialog(result As Boolean)
    Event MustRebind(target As String)

    Property View As IView
        Get
            Return _View
        End Get
        Set
            _View = Value
            _measurementEditViewModel.View = _View
        End Set
    End Property

    Sub New()
        HasWell = False
        NameEditable = True
        _well = New Well()
        _measurementEditViewModel = New EditMeasurementViewModel(_well)
        _repo = RepositoryWrapper.Instance.Wells
    End Sub

    Sub New(w As Well)
        _well = w
        _measurementEditViewModel = New EditMeasurementViewModel(_well)
        HasWell = True
        NameEditable = False
        InitializeWell()
        _repo = RepositoryWrapper.Instance.Wells
    End Sub

    Private Sub InitializeWell()
        WellName = _well.Name
        X = _well.X
        Y = _well.Y
        Z = _well.Z
        Latitude = _well.Latitude
        Longitude = _well.Longitude
        Type = _well.WellType
        Height = _well.Height
        Exists = _well.Exists
        Bottom = _well.Bottom
        Analysis = _well.Analyses
        Measurements = _well.Measurements
        Links = _well.Files

        Measurements.Sort()
        Analysis.Sort()
        Links.Sort()
    End Sub

    Private Sub CreateOrEditWell()
        If Not HasWell Then
            _well.Name = WellName
        End If

        _well.X = X
        _well.Y = Y
        _well.Z = Z
        _well.Latitude = Latitude
        _well.Longitude = Longitude
        _well.WellType = Type
        _well.Bottom = Bottom
        _well.Exists = Exists
        _well.Height = Height
        _well.Analyses = Analysis
        _well.Measurements = Measurements
        _well.Files = Links
    End Sub

    Private Function Validate()
        If HasWell Then
            Return True
        Else
            If Not String.IsNullOrEmpty(WellName) Then
                Return Not _repo.ContainsName(WellName)
            End If
        End If
        Return False
    End Function

    Private Sub RemoveAll()

        RepositoryWrapper.Instance.Measurements.RemoveRange(_well.Measurements)
        RepositoryWrapper.Instance.Analyses.RemoveRange(_well.Analyses)
        RepositoryWrapper.Instance.ExternalFiles.RemoveRange(_well.Files)
        _repo.Remove(_well)

    End Sub

    ReadOnly Property DeleteWellCommand As ICommand = New RelayCommand(Sub()
                                                                           If View.ShowMessageBox("¿Desea eliminar este pozo? Esto borrará toda la información asociada al mismo.", "Borrar pozo") Then
                                                                               RemoveAll()
                                                                               RepositoryWrapper.Instance.Save()
                                                                               RaiseEvent CloseDialog(True)
                                                                           End If
                                                                       End Sub,
                                                                   Function()
                                                                       Return HasWell
                                                                   End Function,
                                                                   AddressOf OnError)

    ReadOnly Property AcceptCommand As ICommand = New RelayCommand(Sub()
                                                                       CreateOrEditWell()
                                                                       If HasWell Then
                                                                           _repo.Update(_well)
                                                                       Else
                                                                           _repo.Add(_well)
                                                                       End If
                                                                       RepositoryWrapper.Instance.Save()
                                                                       RaiseEvent CloseDialog(True)
                                                                   End Sub,
                                                                   Function() Validate(),
                                                                   AddressOf OnError)


    Property NewMeasurementCommand As ICommand = New RelayCommand(Sub()
                                                                      Dim result = CType(_View, WellEditingDialog).ShowEditMeasurementDialog(_measurementEditViewModel)
                                                                      If result Then
                                                                          Measurements.Sort()
                                                                          RaiseEvent MustRebind(NameOf(Measurements))
                                                                      End If
                                                                  End Sub,
                                                      Function() HasWell, AddressOf OnError)

    Property EditMeasurementCommand As ICommand = New RelayCommand(Sub(param)
                                                                       If param IsNot Nothing AndAlso TypeOf param Is Measurement Then
                                                                           Dim vm As New EditMeasurementViewModel(CType(param, Measurement))
                                                                           Dim result = CType(_View, WellEditingDialog).ShowEditMeasurementDialog(vm)
                                                                           If result Then
                                                                               Measurements.Sort()
                                                                               RaiseEvent MustRebind(NameOf(Measurements))
                                                                           End If
                                                                       End If
                                                                   End Sub,
                                                      Function() HasWell, AddressOf OnError)

    Property DeleteMeasurementCommand As ICommand = New RelayCommand(Sub(param)
                                                                         If param IsNot Nothing AndAlso TypeOf param Is Measurement Then
                                                                             Dim measurement = CType(param, Measurement)
                                                                             _well.Measurements.Remove(measurement)
                                                                             RepositoryWrapper.Instance.Measurements.Remove(measurement)
                                                                             Measurements.Remove(measurement)
                                                                             Measurements.Sort()
                                                                             RaiseEvent MustRebind(NameOf(Measurements))
                                                                         End If
                                                                     End Sub,
                                                      Function() HasWell, AddressOf OnError)

    Property NewExternalLinkCommand As ICommand = New RelayCommand(Sub()
                                                                       Dim filename = _View.OpenFileDialog("Archivos|*.*", "Elija un archivo")
                                                                       If Not String.IsNullOrEmpty(filename) Then
                                                                           Dim ExternalLink = New ExternalFile() With {.Well = _well}
                                                                           RepositoryWrapper.Instance.ExternalFiles.Add(ExternalLink, filename)
                                                                           Links.Sort()
                                                                           RaiseEvent MustRebind(NameOf(Links))
                                                                       End If
                                                                   End Sub,
                                                      Function() HasWell, AddressOf OnError)

    Property OpenExternalLinkCommand As ICommand = New RelayCommand(Sub(param)
                                                                        If param IsNot Nothing AndAlso TypeOf param Is ExternalFile Then
                                                                            Dim link = CType(param, ExternalFile)
                                                                            link.Open()
                                                                        End If
                                                                    End Sub,
                                                      Function() HasWell, AddressOf OnError)

    Property DeleteExternalLinkCommand As ICommand = New RelayCommand(Sub(param)
                                                                          If param IsNot Nothing AndAlso TypeOf param Is ExternalFile Then
                                                                              If _View.ShowMessageBox("Esto eliminará el archivo vinculado a la base de datos. ¿Desea continuar?", "Eliminar archivo") Then
                                                                                  Dim link = CType(param, ExternalFile)
                                                                                  _well.Files.Remove(link)
                                                                                  RepositoryWrapper.Instance.ExternalFiles.Remove(link)
                                                                                  Links.Remove(link)
                                                                                  Links.Sort()
                                                                                  RaiseEvent MustRebind(NameOf(Links))
                                                                              End If
                                                                          End If
                                                                      End Sub,
                                                      Function() HasWell, AddressOf OnError)

End Class
