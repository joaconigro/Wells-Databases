Imports Wells.Model

Public Class PrecipitationsRepository
    Inherits Repository(Of Precipitation)

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of Precipitation))
        MyBase.New(context, repositories, entities)
    End Sub

    Public Overrides Function Add(entity As Precipitation) As Boolean
        If _entities.ContainsKey(entity.Id) Then
            Return False
        Else
            _entities.Add(entity.Id, entity)
            Return True
        End If
    End Function
End Class
