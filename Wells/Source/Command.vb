Public Class Command
    Implements ICommand

    Private _execute As Action(Of Object)
    Private _canExecute As Func(Of Object, Boolean) = Function(parameters) True
    Private _onError As Action(Of Exception)

    Sub New(execute As Action(Of Object), canExecute As Func(Of Object, Boolean))
        _execute = execute
        _canExecute = canExecute
    End Sub

    Sub New(execute As Action(Of Object), canExecute As Func(Of Object, Boolean), onError As Action(Of Exception))
        _execute = execute
        _canExecute = canExecute
        _onError = onError
    End Sub

    Sub New(execute As Action(Of Object))
        _execute = execute
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub InternalExecute(parameter As Object) Implements ICommand.Execute
        If CanExecute(parameter) Then
            Try
                _execute(parameter)
            Catch ex As Exception
                If _onError Is Nothing Then
                    Throw ex
                Else
                    _onError(ex)
                End If
            End Try
        End If
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return _canExecute(parameter)
    End Function

    Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, New EventArgs)
    End Sub
End Class
