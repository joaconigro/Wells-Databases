Imports FluentValidation

Public Class EmptyStringValidator
    Inherits AbstractValidator(Of String)
    Sub New(propertyDisplayName As String)
        RuleFor(Function(text) text).
            Must(Function(text) Not String.IsNullOrEmpty(text)).WithMessage($"{propertyDisplayName}: no puede quedar vacío.")
    End Sub
End Class
