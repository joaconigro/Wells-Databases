Imports FluentValidation

Public Class NullObjectValidator
    Inherits AbstractValidator(Of Object)
    Sub New(propertyDisplayName As String)
        RuleFor(Function(o) o).
            Must(Function(o) o IsNot Nothing).WithMessage($"{propertyDisplayName}: no puede ser nulo.")
    End Sub
End Class
