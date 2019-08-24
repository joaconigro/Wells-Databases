Imports FluentValidation

Public Class EmptyListValidator(Of T)
    Inherits AbstractValidator(Of IEnumerable(Of T))
    Sub New(propertyDisplayName As String)
        RuleFor(Function(list) list).
            Must(Function(list) list IsNot Nothing AndAlso list.Any).WithMessage($"{propertyDisplayName}: debe tener al menos un elemento.")
    End Sub
End Class
