Imports Wells.Model

Public Class RejectedEntity
    Public Enum RejectedReasons
        None
        DuplicatedId
        DuplicatedName
        WellNameEmpty
        WellNotFound
        FLNADepthGreaterThanWaterDepth
        DuplicatedDate
        Unknown
    End Enum

    ReadOnly Property Entity As IBusinessObject
    ReadOnly Property OriginalRow As Integer
    ReadOnly Property Reason As RejectedReasons

    Sub New(entity As IBusinessObject, originalRow As Integer, reason As RejectedReasons)
        Me.Entity = entity
        Me.OriginalRow = originalRow
        Me.Reason = reason
    End Sub

End Class
