Imports Wells.Model

Public Class ExternalLinksRepository
    Inherits Repository(Of ExternalLink)

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of ExternalLink))
        MyBase.New(context, repositories, entities)
    End Sub

    Public Overrides Function Add(entity As ExternalLink, ByRef reason As RejectedEntity.RejectedReasons) As Boolean
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
            Else
                _entities.Add(entity.Id, entity)
                Dim well = _repositories.Wells.FindName(entity.WellName)
                well.Links.Add(entity)
                entity.Well = well
                Return True
            End If
        Catch ex As Exception
            reason = RejectedEntity.RejectedReasons.Unknown
            Return False
        End Try
    End Function
End Class
