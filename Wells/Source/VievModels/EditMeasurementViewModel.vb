Imports Wells.Model
Imports Wells.Persistence

Public Class EditMeasurementViewModel
    Inherits BaseViewModel

    Private _WellName As String
    Property WellName As String
        Get
            Return _WellName
        End Get
        Set
            _WellName = Value
            CType(AcceptCommand, Command).RaiseCanExecuteChanged()
        End Set
    End Property

    Property RealDate As Date
    Property FLNADepth As Double
    Property WaterDepth As Double
    Property Caudal As Double
    Property Comment As String

    ReadOnly Property HasMeasurement As Boolean
    ReadOnly Property NameSelectable As Boolean

    Private _measurement As Measurement
    Private _repo As MeasurementsRepository
    Private _well As Well
    Private _SelectedWellIndex As Integer

    ReadOnly Property WellNames As List(Of String)
        Get
            Return Repositories.Instance?.Wells.Names
        End Get
    End Property

    Property SelectedWellIndex As Integer
        Get
            Return WellNames.IndexOf(_WellName)
        End Get
        Set
            _SelectedWellIndex = Value
            WellName = WellNames(_SelectedWellIndex)
            _well = Repositories.Instance.Wells.FindName(_WellName)
        End Set
    End Property

    Event CloseDialog(result As Boolean)

    Property View As IView

    Sub New()
        HasMeasurement = False
        NameSelectable = True
        _repo = Repositories.Instance.Measurements
    End Sub

    Sub New(w As Well)
        HasMeasurement = False
        NameSelectable = False
        _well = w
        _WellName = w.Name
        _repo = Repositories.Instance.Measurements
    End Sub

    Sub New(m As Measurement)
        _measurement = m
        HasMeasurement = True
        NameSelectable = False
        InitializeMeasurement()
        _well = m.Well
        _repo = Repositories.Instance.Measurements
    End Sub

    Private Sub InitializeMeasurement()
        WellName = _measurement.WellName
        RealDate = _measurement.RealDate
        FLNADepth = _measurement.FLNADepth
        WaterDepth = _measurement.WaterDepth
        Caudal = _measurement.Caudal
        Comment = _measurement.Comment
    End Sub

    Private Sub CreateOrEditMeasurement()
        If HasMeasurement Then
            _measurement.Well = _well
            _measurement.WellName = WellName
            _measurement.SampleDate = RealDate.ToString("dd/MM/yyyy")
            _measurement.FLNADepth = FLNADepth
            _measurement.WaterDepth = WaterDepth
            _measurement.Caudal = Caudal
            _measurement.Comment = Comment
        Else
            _measurement = New Measurement() With {
                .Well = _well,
                .WellName = _WellName,
                .SampleDate = RealDate.ToString("dd/MM/yyyy"),
                .FLNADepth = FLNADepth,
                .WaterDepth = WaterDepth,
                .Caudal = Caudal,
                .Comment = Comment}
        End If
    End Sub

    Private Function Validate()
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
        _well.Measurements.Remove(_measurement)
        _repo.Remove(_measurement)
    End Sub

    ReadOnly Property DeleteMeasurementCommand As ICommand = New Command(Sub()
                                                                             If View.ShowMessageBox("¿Desea eliminar este medición?", "Borrar medición") Then
                                                                                 RemoveAll()
                                                                                 Repositories.Instance.SaveChanges()
                                                                                 RaiseEvent CloseDialog(True)
                                                                             End If
                                                                         End Sub,
                                                                   Function()
                                                                       Return HasMeasurement
                                                                   End Function,
                                                                   AddressOf OnError)

    ReadOnly Property AcceptCommand As ICommand = New Command(Sub()
                                                                  CreateOrEditMeasurement()
                                                                  If HasMeasurement Then
                                                                      _repo.Update(_measurement)
                                                                  Else
                                                                      If _repo.Add(_measurement, RejectedEntity.RejectedReasons.None) Then
                                                                          '_well.Measurements.Add(_measurement)
                                                                      End If
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
