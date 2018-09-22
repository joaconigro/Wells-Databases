Imports Wells.Model

Public Class MeasurementsRepository
    Inherits Repository(Of Measurement)

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of Measurement))
        MyBase.New(context, repositories, entities)
    End Sub

    Public Overrides Function Add(entity As Measurement, ByRef reason As RejectedEntity.RejectedReasons) As Boolean
        Try
            If String.IsNullOrEmpty(entity.WellName) Then
                reason = RejectedEntity.RejectedReasons.WellNameEmpty
                Return False
            ElseIf Not _repositories.Wells.ContainsName(entity.WellName) Then
                reason = RejectedEntity.RejectedReasons.WellNotFound
                Return False
            ElseIf _entities.ContainsKey(entity.Id) Then
                reason = RejectedEntity.RejectedReasons.DuplicatedId
                Return False
            ElseIf entity.FLNADepth > entity.WaterDepth Then
                reason = RejectedEntity.RejectedReasons.FLNADepthGreaterThanWaterDepth
                Return False
            Else
                _entities.Add(entity.Id, entity)
                Dim well = _repositories.Wells.FindName(entity.WellName)
                well.Measurements.Add(entity)
                entity.Well = well
                Return True
            End If
        Catch ex As Exception
            reason = RejectedEntity.RejectedReasons.Unknown
            Return False
        End Try
    End Function
End Class
