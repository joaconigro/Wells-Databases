Imports Wells.Model

Public Interface IRepository(Of T As IBusinessObject)
    ReadOnly Property All As List(Of T)
    Sub Delete(entity As IBusinessObject)
    Sub Update(entity As IBusinessObject)
    Function Add(entity As T) As Boolean
    Function AddRange(entities As IEnumerable(Of T)) As List(Of T)
    Function Exists(id As String) As Boolean
    Function Find(id As String) As T
End Interface
