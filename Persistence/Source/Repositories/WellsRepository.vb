Imports Wells.Model

Public Class WellsRepository
    Inherits Repository(Of Well)

    Private _names As Dictionary(Of String, Well)

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of Well))
        MyBase.New(context, repositories, entities)
        _names = entities.ToDictionary(Function(e) e.Name)
    End Sub

    ReadOnly Property Names As List(Of String)
        Get
            Dim list = _names.Keys.ToList
            list.Sort()
            Return list
        End Get
    End Property

    Public Overrides Function Add(entity As Well, ByRef reason As RejectedEntity.RejectedReasons) As Boolean
        Try
            If String.IsNullOrEmpty(entity.Name) Then
                reason = RejectedEntity.RejectedReasons.WellNameEmpty
                Return False
            ElseIf _entities.ContainsKey(entity.Id) Then
                reason = RejectedEntity.RejectedReasons.DuplicatedId
                Return False
            ElseIf _names.ContainsKey(entity.Name) Then
                reason = RejectedEntity.RejectedReasons.DuplicatedName
                Return False
            Else
                _entities.Add(entity.Id, entity)
                _names.Add(entity.Name, entity)
                Return True
            End If
        Catch ex As Exception
            reason = RejectedEntity.RejectedReasons.Unknown
            Return False
        End Try
    End Function

    Function FindName(name As String) As Well
        If _names.ContainsKey(name) Then
            Return _names(name)
        End If
        Return Nothing
    End Function

    Function ContainsName(name As String) As Boolean
        Return _names.ContainsKey(name)
    End Function
End Class
