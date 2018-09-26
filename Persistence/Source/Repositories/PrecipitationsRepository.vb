Imports Wells.Model

Public Class PrecipitationsRepository
    Inherits Repository(Of Precipitation)

    Private _dates As Dictionary(Of Date, Precipitation)

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of Precipitation))
        MyBase.New(context, repositories, entities)
        _dates = entities.ToDictionary(Function(e) e.RealDate)
    End Sub

    Protected Overrides Sub InternalRemove(entity As Precipitation)
        _entities.Remove(entity.Id)
        _dates.Remove(entity.RealDate)
    End Sub

    Public Overrides Function Add(entity As Precipitation, ByRef reason As RejectedEntity.RejectedReasons) As Boolean
        Try
            If _entities.ContainsKey(entity.Id) Then
                reason = RejectedEntity.RejectedReasons.DuplicatedId
                Return False
            ElseIf _dates.Containskey(entity.RealDate) Then
                reason = RejectedEntity.RejectedReasons.DuplicatedDate
                Return False
            Else
                _entities.Add(entity.Id, entity)
                _dates.Add(entity.RealDate, entity)
                Return True
            End If
        Catch ex As Exception
            reason = RejectedEntity.RejectedReasons.Unknown
            Return False
        End Try
    End Function
End Class
