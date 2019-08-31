Imports System.Text.RegularExpressions
Imports FluentValidation
Imports Wells.CorePersistence.Repositories

Public Class EmailValidator
    Inherits AbstractValidator(Of String)

    Private _CheckIfExist As Boolean

    Sub New(checkIfExist As Boolean)
        _CheckIfExist = checkIfExist
        RuleFor(Function(text) text).Must(Function(text) Not String.IsNullOrEmpty(text)).WithMessage("El mail no puede quedar vacío.")
        RuleFor(Function(mail) mail).Must(Function(mail) MatchEmail(mail)).WithMessage("Este mail no tiene un formato válido.")
        RuleFor(Function(mail) mail).Must(Function(mail) Not MailExists(mail)).WithMessage("Este mail ya existe en la base de datos.")
    End Sub

    Private Function MatchEmail(mail As String) As Boolean
        Dim emailExpression As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        Return emailExpression.IsMatch(mail)
    End Function

    Private Function MailExists(mail As String) As Boolean
        'If _CheckIfExist Then
        '    Return CType(RepositoryWrapper.Instance.ClientUsersRepository, IClientUsersRepository).ContainsEmail(mail)
        'End If
        Return False
    End Function
End Class
