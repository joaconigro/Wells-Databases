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

    Function AddRange(entities As IEnumerable(Of T), progress As IProgress(Of Integer)) As List(Of RejectedEntity) Implements IRepository(Of T).AddRange
        Dim rejecteds As New List(Of RejectedEntity)
        Dim inserted As New List(Of T)
        Dim reason As RejectedEntity.RejectedReasons
        Dim total = entities.Count
        For i = 0 To total - 1
            Dim e = entities(i)
            reason = RejectedEntity.RejectedReasons.None
            If Add(e, reason) Then
                inserted.Add(e)
            Else
                rejecteds.Add(New RejectedEntity(e, i + 2, reason))
            End If
            progress.Report((i + 1) / total * 100)
        Next
        _context.AddRange(inserted)

        Return rejecteds
    End Function

    Protected MustOverride Sub InternalRemove(entity As T) Implements IRepository(Of T).InternalRemove

    Sub Update(entity As T) Implements IRepository(Of T).Update
        _context.Entry(entity).State = EntityState.Modified
    End Sub

    Sub Remove(entity As T) Implements IRepository(Of T).Remove
        Dim entityType = entity.GetType
        InternalRemove(entity)
        _context.Set(entityType).Remove(entity)
    End Sub

    Sub RemoveRange(entities As IEnumerable(Of T)) Implements IRepository(Of T).RemoveRange
        If entities.Any Then
            Dim entityType = entities.First.GetType
            For Each e In entities
                InternalRemove(e)
            Next
            _context.Set(entityType).RemoveRange(entities)
        End If
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
