Imports Wells.Model

Public Class ExternalLinksRepository
    Inherits Repository(Of ExternalLink)

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of ExternalLink))
        MyBase.New(context, repositories, entities)
    End Sub

    Public Overrides Function Add(entity As ExternalLink) As Boolean
        If Not _repositories.Wells.ContainsName(entity.WellName) OrElse _entities.ContainsKey(entity.Id) Then
            Return False
        Else
            _entities.Add(entity.Id, entity)
            Dim well = _repositories.Wells.FindName(entity.WellName)
            well.Links.Add(entity)
            entity.Well = well
            Return True
        End If
    End Function
End Class
