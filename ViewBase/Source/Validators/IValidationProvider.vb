Imports FluentValidation.Results

Public Interface IValidatorProvider
    Function ValidationResult(propertyName As String) As ValidationResult
    Function IsValid(propertyName As String) As Boolean
End Interface
