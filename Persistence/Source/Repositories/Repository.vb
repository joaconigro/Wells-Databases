Imports System.Data.Entity
Imports Wells.Model

Public MustInherit Class Repository(Of T As IBusinessObject)
    Implements IRepository(Of T)

    Protected _entities As Dictionary(Of String, T)
    Protected _context As Context
    Protected _repositories As Repositories

    Sub New(context As Context, repositories As Repositories, entities As IEnumerable(Of T))
        _context = context
        _repositories = repositories
        _entities = entities.ToDictionary(Function(e) e.Id)
    End Sub

    ReadOnly Property All As List(Of T) Implements IRepository(Of T).All
        Get
            Return _entities.Values.ToList
        End Get
    End Property

    MustOverride Function Add(entity As T, ByRef reason As RejectedEntity.RejectedReasons) As Boolean Implements IRepository(Of T).Add

    Function AddRange(entities As IEnumerable(Of T)) As List(Of RejectedEntity) Implements IRepository(Of T).AddRange
        Dim rejecteds As New List(Of RejectedEntity)
        Dim inserted As New List(Of T)
        Dim reason As RejectedEntity.RejectedReasons
        For Each e In entities
            reason = RejectedEntity.RejectedReasons.None
            If Add(e, reason) Then
                inserted.Add(e)
            Else
                Dim index = entities.ToList.IndexOf(e) + 2
                rejecteds.Add(New RejectedEntity(e, index, reason))
            End If
        Next
        _context.AddRange(inserted)

        Return rejecteds
    End Function

    Sub Update(entity As IBusinessObject) Implements IRepository(Of T).Update
        _context.Entry(entity).State = EntityState.Modified
    End Sub

    Sub Delete(entity As IBusinessObject) Implements IRepository(Of T).Delete
        Dim entityType = entity.GetType
        _context.Set(entityType).Remove(entity)
    End Sub

    Function Find(id As String) As T Implements IRepository(Of T).Find
        If _entities.ContainsKey(id) Then
            Return _entities(id)
        End If
        Return Nothing
    End Function

    Function Exists(id As String) As Boolean Implements IRepository(Of T).Exists
        Return _entities.ContainsKey(id)
    End Function

End Class
