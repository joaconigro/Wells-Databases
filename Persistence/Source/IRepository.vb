Imports Wells.Model

Public Interface IRepository

    Sub Add(entity As IBusinessObject)
    Sub Update(entity As IBusinessObject)
    Sub Delete(entity As IBusinessObject)

End Interface
