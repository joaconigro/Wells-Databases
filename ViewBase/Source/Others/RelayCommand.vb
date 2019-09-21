Imports System.Windows.Input

Public Class RelayCommand
    Implements ICommand

    Private ReadOnly _Execute As Action(Of Object)
    Private ReadOnly _CanExecute As Func(Of Object, Boolean) = Function(parameters) True
    Private ReadOnly _OnError As Action(Of Exception)
    Private ReadOnly _Finally As Action

    Sub New(execute As Action(Of Object), canExecute As Func(Of Object, Boolean))
        _Execute = execute
        _CanExecute = canExecute
    End Sub

    Sub New(execute As Action(Of Object), canExecute As Func(Of Object, Boolean), onError As Action(Of Exception), Optional [finally] As Action = Nothing)
        _Execute = execute
        _CanExecute = canExecute
        _OnError = onError
        _Finally = [finally]
    End Sub

    Sub New(execute As Action(Of Object))
        _Execute = execute
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub InternalExecute(parameter As Object) Implements ICommand.Execute
        If CanExecute(parameter) Then
            Try
                _Execute(parameter)
            Catch ex As Exception
                If _OnError Is Nothing Then
                    Throw ex
                Else
                    _OnError(ex)
                End If
            Finally
                _Finally?.Invoke
            End Try
        End If
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return _CanExecute(parameter)
    End Function

    Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, New EventArgs)
    End Sub
End Class
