Imports Wells.Model
Imports Wells.Persistence

Public Class EditWellViewModel
    Inherits BaseViewModel

    Property WellName As String
        Get
            Return _WellName
        End Get
        Set
            _WellName = Value
            CType(AcceptCommand, Command).RaiseCanExecuteChanged()
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
    Property Links As New List(Of ExternalLink)

    ReadOnly Property HasWell As Boolean
    ReadOnly Property NameEditable As Boolean
    ReadOnly Property WellTypes As New List(Of String) From {"Pozo", "Sondeo"}

    Private _well As Well
    Private _repo As WellsRepository
    Private _WellName As String
    Event CloseDialog(result As Boolean)

    Property View As IView

    Sub New()
        HasWell = False
        NameEditable = True
        _repo = Repositories.Instance.Wells
    End Sub

    Sub New(w As Well)
        _well = w
        HasWell = True
        NameEditable = False
        InitializeWell()
        _repo = Repositories.Instance.Wells
    End Sub

    Private Sub InitializeWell()
        WellName = _well.Name
        X = _well.X
        Y = _well.Y
        Z = _well.Z
        Latitude = _well.Latitude
        Longitude = _well.Longitude
        Type = _well.Type
        Height = _well.Height
        Exists = _well.Exists
        Bottom = _well.Bottom
        Analysis = _well.Analysis
        Measurements = _well.Measurements
        Links = _well.Links

        Measurements.Sort()
        Analysis.Sort()
        Links.Sort()
    End Sub

    Private Sub CreateOrEditWell()
        If HasWell Then
            _well.X = X
            _well.Y = Y
            _well.Z = Z
            _well.Latitude = Latitude
            _well.Longitude = Longitude
            _well.Type = Type
            _well.Bottom = Bottom
            _well.Exists = Exists
            _well.Height = Height
            _well.Analysis = Analysis
            _well.Measurements = Measurements
            _well.Links = Links
        Else
            _well = New Well(WellName) With {
                .X = X,
                .Y = Y,
                .Z = Z,
                .Latitude = Latitude,
                .Longitude = Longitude,
                .Type = Type,
                .Bottom = Bottom,
                .Exists = Exists,
                .Height = Height,
                .Analysis = Analysis,
                .Measurements = Measurements,
                .Links = Links}
        End If
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

        Repositories.Instance.Measurements.RemoveRange(_well.Measurements)
        Repositories.Instance.ChemicalAnalysis.RemoveRange(_well.Analysis)
        Repositories.Instance.Links.RemoveRange(_well.Links)
        _repo.Remove(_well)

    End Sub

    ReadOnly Property DeleteWellCommand As ICommand = New Command(Sub()
                                                                      If View.ShowMessageBox("¿Desea eliminar este pozo? Esto borrará toda la información asociada al mismo.", "Borrar pozo") Then
                                                                          RemoveAll()
                                                                          Repositories.Instance.SaveChanges()
                                                                          RaiseEvent CloseDialog(True)
                                                                      End If
                                                                  End Sub,
                                                                   Function()
                                                                       Return HasWell
                                                                   End Function,
                                                                   AddressOf OnError)

    ReadOnly Property AcceptCommand As ICommand = New Command(Sub()
                                                                  CreateOrEditWell()
                                                                  If HasWell Then
                                                                      _repo.Update(_well)
                                                                  Else
                                                                      _repo.Add(_well, RejectedEntity.RejectedReasons.None)
                                                                  End If
                                                                  Repositories.Instance.SaveChanges()
                                                                  RaiseEvent CloseDialog(True)
                                                              End Sub,
                                                                   Function() Validate(),
                                                                   AddressOf OnError)

    ReadOnly Property CancelCommand As ICommand = New Command(Sub() RaiseEvent CloseDialog(False))

    Protected Overrides Sub ShowErrorMessage(message As String)
        View.ShowErrorMessageBox(message)
    End Sub
End Class
