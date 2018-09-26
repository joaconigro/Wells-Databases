Imports Wells.Model

Public Interface IRepository(Of T As IBusinessObject)
    ReadOnly Property All As List(Of T)
    Sub InternalRemove(entity As T)
    Sub Remove(entity As T)
    Sub RemoveRange(entities As IEnumerable(Of T))
    Sub Update(entity As T)
    Function Add(entity As T, ByRef reason As RejectedEntity.RejectedReasons) As Boolean
    Function AddRange(entities As IEnumerable(Of T), progress As IProgress(Of Integer)) As List(Of RejectedEntity)
    Function Exists(id As String) As Boolean
    Function Find(id As String) As T
End Interface
