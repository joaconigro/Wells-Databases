Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Input
Imports FluentValidation
Imports FluentValidation.Results
Imports Wells.Base

Public MustInherit Class BaseViewModel
    Implements INotifyPropertyChanged, IValidatorProvider

    Protected validators As New Dictionary(Of String, IValidator)
    Protected failures As New Dictionary(Of String, ValidationResult)
    Protected validationResults As New Dictionary(Of String, ValidationResult)
    Protected commandNotifications As New Dictionary(Of String, IEnumerable(Of ICommand))

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub NotifyPropertyChanged(propertyName As String)
        RaiseCommandUpdates(propertyName)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Protected Sub ShowErrorMessage(message As String)
        View?.ShowErrorMessageBox(message)
    End Sub

    Protected Sub ShowMessage(message As String, title As String)
        View?.ShowMessageBox(message, title)
    End Sub

    Protected Sub OnError(ex As Exception)
        Application.Current.Dispatcher.Invoke(Sub()
                                                  ExceptionHandler.Handle(ex, False)
                                                  Dim errorMessage = ExceptionHandler.GetAllMessages(ex).Trim
                                                  ShowErrorMessage(errorMessage)
                                              End Sub)
    End Sub

    Protected Overridable Property View As IView

    Sub New(view As IView)
        If view IsNot Nothing Then
            SetView(view)
        End If
        _progress = New Progress(Of Integer)(AddressOf ProgressChanged)
    End Sub

    Sub SetView(view As IView)
        Me.View = view
        NotifyPropertyChanged(NameOf(CloseModalViewCommand))
        NotifyPropertyChanged(NameOf(CloseNonModalViewCommand))
        OnSetView(view)
    End Sub

    Protected Overridable Sub OnSetView(view As IView)

    End Sub

    ReadOnly Property Errors As String
        Get
            Return String.Join(vbCrLf, From v In validationResults.Values
                                       Where Not v.IsValid
                                       Select String.Join(vbCrLf, v.Errors.Select(Function(e) e.ErrorMessage)))
        End Get
    End Property

    ReadOnly Property HasFailures As Boolean
        Get
            If validators.Any AndAlso validationResults.Count <> validators.Count Then
                Return True
            End If
            Return validationResults.Values.Any(Function(r) Not r.IsValid)
        End Get
    End Property

    Protected Overridable Sub Initialize()
        SetValidators()
        SetCommandUpdates()
    End Sub

    Protected MustOverride Sub SetValidators()

    Protected MustOverride Sub SetCommandUpdates()

    Protected Sub ValidateAll()
        For Each key In validators.Keys
            Validate(key, CallByName(Me, key, CallType.Get))
        Next
    End Sub

    Protected Sub Validate(Of T)(propertyName As String, value As T)
        validationResults(propertyName) = validators(propertyName).Validate(value)
        If IsValid(propertyName) Then
            OnPropertyValidated(propertyName)
        End If
    End Sub

    Sub AddValidator(propertyName As String, validator As IValidator)
        validators.Add(propertyName, validator)
    End Sub

    Sub AddCommands(propertyName As String, commands As IEnumerable(Of ICommand))
        If commandNotifications.ContainsKey(propertyName) Then
            Dim newlist = commandNotifications(propertyName).ToList
            newlist.AddRange(commands)
            commandNotifications(propertyName) = newlist
        Else
            commandNotifications.Add(propertyName, commands)
        End If
    End Sub

    Protected Sub SetValue(Of T)(ByRef value As T, newValue As T, <CallerMemberName()> Optional propertyName As String = Nothing, Optional forceAssignment As Boolean = False)
        If (newValue Is Nothing And value IsNot Nothing) OrElse (newValue IsNot Nothing AndAlso (Not newValue.Equals(value) Or forceAssignment)) Then
            OnPropertyChanging(propertyName)
            value = newValue
            If validators.ContainsKey(propertyName) Then
                Validate(propertyName, newValue)
            End If
            NotifyPropertyChanged(propertyName)
            OnPropertyChanged(propertyName)
            UpdateCommandsThatDependsOnFailures()
        End If
    End Sub

    Protected Overridable Sub OnPropertyChanging(propertyName As String)

    End Sub

    Protected Overridable Sub OnPropertyChanged(propertyName As String)

    End Sub

    Protected Overridable Sub OnPropertyValidated(propertyName As String)

    End Sub

    Protected Overridable Sub RaiseCommandUpdates(propertyName As String)
        If commandNotifications.ContainsKey(propertyName) Then
            For Each com In commandNotifications(propertyName)
                CType(com, RelayCommand).RaiseCanExecuteChanged()
            Next
        End If
    End Sub

    Protected Sub UpdateCommandsThatDependsOnFailures()
        NotifyPropertyChanged(NameOf(Errors))
        NotifyPropertyChanged(NameOf(HasFailures))
        RaiseCommandUpdates(NameOf(Errors))
        RaiseCommandUpdates(NameOf(HasFailures))
    End Sub

    Function ValidationResult(propertyName As String) As ValidationResult Implements IValidatorProvider.ValidationResult
        Dim result As ValidationResult = Nothing
        If propertyName Is Nothing OrElse Not validationResults.TryGetValue(propertyName, result) Then
            Return New ValidationResult()
        Else
            Return result
        End If
    End Function

    Function IsValid(propertyName As String) As Boolean Implements IValidatorProvider.IsValid
        If propertyName Is Nothing Then Return True
        Return ValidationResult(propertyName).IsValid
    End Function

    Property CloseModalViewCommand As ICommand = New RelayCommand(Sub(param)
                                                                      Dim result As Boolean?
                                                                      If param Is Nothing Then
                                                                          result = False
                                                                      Else
                                                                          result = CType(param, Boolean?)
                                                                      End If
                                                                      View?.CloseView(result)
                                                                  End Sub, Function()
                                                                               Return View IsNot Nothing
                                                                           End Function, AddressOf OnError)

    Property CloseNonModalViewCommand As ICommand = New RelayCommand(Sub()
                                                                         View?.CloseView()
                                                                     End Sub, Function()
                                                                                  Return View IsNot Nothing
                                                                              End Function, AddressOf OnError)


    Protected WithEvents _progress As IProgress(Of Integer)
    Protected _processInfo As String
    ReadOnly Property ProcessInfo As String
        Get
            Return _processInfo
        End Get
    End Property

    Protected _showProgress As Boolean
    ReadOnly Property ShowProgress As Boolean
        Get
            Return _showProgress
        End Get
    End Property

    Protected _undefinedProgress As Boolean
    ReadOnly Property UndefinedProgress As Boolean
        Get
            Return _undefinedProgress
        End Get
    End Property

    Protected _progressValue As Integer
    ReadOnly Property ProgressValue As Integer
        Get
            Return _progressValue
        End Get
    End Property

    Protected Sub ProgressChanged(value As Integer)
        _progressValue = value
        NotifyPropertyChanged(NameOf(ProgressValue))
    End Sub

    Protected Sub StartProgressNotifications(undefined As Boolean, Optional message As String = "")
        _progressValue = 0
        _undefinedProgress = undefined
        _processInfo = message
        _showProgress = True
        NotifyPropertyChanged(NameOf(ProgressValue))
        NotifyPropertyChanged(NameOf(ProcessInfo))
        NotifyPropertyChanged(NameOf(UndefinedProgress))
        NotifyPropertyChanged(NameOf(ShowProgress))
    End Sub

    Protected Sub StopProgressNotifications()
        _undefinedProgress = False
        _processInfo = String.Empty
        _showProgress = False
        NotifyPropertyChanged(NameOf(ProcessInfo))
        NotifyPropertyChanged(NameOf(UndefinedProgress))
        NotifyPropertyChanged(NameOf(ShowProgress))
    End Sub

End Class
