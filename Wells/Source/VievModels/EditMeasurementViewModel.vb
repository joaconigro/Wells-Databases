Imports Wells.YPFModel
Imports Wells.CorePersistence
Imports Wells.ViewBase
Imports Wells.CorePersistence.Repositories

Public Class EditMeasurementViewModel
    Inherits BaseViewModel

    Private _WellName As String
    Property WellName As String
        Get
            Return _WellName
        End Get
        Set
            _WellName = Value
            CType(AcceptCommand, RelayCommand).RaiseCanExecuteChanged()
        End Set
    End Property

    Property RealDate As Date
    Property FLNADepth As Double
    Property WaterDepth As Double
    Property Comment As String

    ReadOnly Property HasMeasurement As Boolean
    ReadOnly Property NameSelectable As Boolean

    Property Measurement As Measurement
    Private _repo As MeasurementsRepository
    Private _well As Well
    Private _SelectedWellIndex As Integer

    ReadOnly Property WellNames As List(Of String)
        Get
            Return RepositoryWrapper.Instance?.Wells.Names
        End Get
    End Property

    Property SelectedWellIndex As Integer
        Get
            Return WellNames.IndexOf(_WellName)
        End Get
        Set
            _SelectedWellIndex = Value
            WellName = WellNames(_SelectedWellIndex)
            _well = RepositoryWrapper.Instance.Wells.FindByName(_WellName)
        End Set
    End Property

    Event CloseDialog(result As Boolean)

    'Property View As IView

    Sub New()
        MyBase.New(Nothing)
        HasMeasurement = False
        NameSelectable = True
        RealDate = Date.Today
        _repo = RepositoryWrapper.Instance.Measurements
    End Sub

    Sub New(w As Well)
        MyBase.New(Nothing)
        HasMeasurement = False
        NameSelectable = False
        RealDate = Date.Today
        _well = w
        _WellName = w.Name
        _repo = RepositoryWrapper.Instance.Measurements
    End Sub

    Sub New(m As Measurement)
        MyBase.New(Nothing)
        Measurement = m
        HasMeasurement = True
        NameSelectable = False
        InitializeMeasurement()
        _well = m.Well
        _repo = RepositoryWrapper.Instance.Measurements
    End Sub

    Private Sub InitializeMeasurement()
        WellName = Measurement.WellName
        RealDate = Measurement.Date
        FLNADepth = Measurement.FLNADepth
        WaterDepth = Measurement.WaterDepth
        Comment = Measurement.Comment
    End Sub

    Private Sub CreateOrEditMeasurement()
        If HasMeasurement Then
            Measurement.Well = _well
            Measurement.Date = RealDate
            Measurement.FLNADepth = FLNADepth
            Measurement.WaterDepth = WaterDepth
            Measurement.Comment = Comment
        Else
            Measurement = New Measurement() With {
                .Well = _well,
                .Date = RealDate,
                .FLNADepth = FLNADepth,
                .WaterDepth = WaterDepth,
                .Comment = Comment}
        End If
    End Sub

    Private Function IsValidMeasurement()
        If HasMeasurement Then
            Return True
        Else
            If Not String.IsNullOrEmpty(WellName) Then
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub RemoveAll()
        _well.Measurements.Remove(Measurement)
        _repo.Remove(Measurement)
    End Sub

    Protected Overrides Sub SetValidators()
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub SetCommandUpdates()
        Throw New NotImplementedException()
    End Sub

    ReadOnly Property DeleteMeasurementCommand As ICommand = New RelayCommand(Sub()
                                                                                  If View.ShowMessageBox("¿Desea eliminar este medición?", "Borrar medición") Then
                                                                                      RemoveAll()
                                                                                      RepositoryWrapper.Instance.SaveChanges()
                                                                                      RaiseEvent CloseDialog(True)
                                                                                  End If
                                                                              End Sub,
                                                                   Function()
                                                                       Return HasMeasurement
                                                                   End Function,
                                                                   AddressOf OnError)

    ReadOnly Property AcceptCommand As ICommand = New RelayCommand(Sub()
                                                                       CreateOrEditMeasurement()
                                                                       If HasMeasurement Then
                                                                           _repo.Update(Measurement)
                                                                       Else
                                                                           _repo.Add(Measurement)
                                                                       End If
                                                                       RepositoryWrapper.Instance.SaveChanges()
                                                                       RaiseEvent CloseDialog(True)
                                                                   End Sub,
                                                                   Function() IsValidMeasurement(),
                                                                   AddressOf OnError)


End Class
