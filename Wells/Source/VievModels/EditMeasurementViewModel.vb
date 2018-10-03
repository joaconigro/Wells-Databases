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

    Property Measurement As Measurement
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
        RealDate = Date.Today
        _repo = Repositories.Instance.Measurements
    End Sub

    Sub New(w As Well)
        HasMeasurement = False
        NameSelectable = False
        RealDate = Date.Today
        _well = w
        _WellName = w.Name
        _repo = Repositories.Instance.Measurements
    End Sub

    Sub New(m As Measurement)
        Measurement = m
        HasMeasurement = True
        NameSelectable = False
        InitializeMeasurement()
        _well = m.Well
        _repo = Repositories.Instance.Measurements
    End Sub

    Private Sub InitializeMeasurement()
        WellName = Measurement.WellName
        RealDate = Measurement.RealDate
        FLNADepth = Measurement.FLNADepth
        WaterDepth = Measurement.WaterDepth
        Caudal = Measurement.Caudal
        Comment = Measurement.Comment
    End Sub

    Private Sub CreateOrEditMeasurement()
        If HasMeasurement Then
            Measurement.Well = _well
            Measurement.WellName = WellName
            Measurement.SampleDate = RealDate.ToString("dd/MM/yyyy")
            Measurement.FLNADepth = FLNADepth
            Measurement.WaterDepth = WaterDepth
            Measurement.Caudal = Caudal
            Measurement.Comment = Comment
        Else
            Measurement = New Measurement() With {
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
        _well.Measurements.Remove(Measurement)
        _repo.Remove(Measurement)
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
                                                                      _repo.Update(Measurement)
                                                                  Else
                                                                      _repo.Add(Measurement, RejectedEntity.RejectedReasons.None)
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
