Imports System.Windows.Input

Public Class AsyncCommand
    Implements IAsyncCommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Private _IsExecuting As Boolean
    Private ReadOnly _Execute As Func(Of Task)
    Private ReadOnly _CanExecute As Func(Of Boolean) = Function() True
    Private ReadOnly _OnError As Action(Of Exception)
    Private ReadOnly _OnFinish As Action

    Sub New(execute As Func(Of Task), canExecute As Func(Of Boolean))
        _Execute = execute
        _CanExecute = canExecute
    End Sub

    Sub New(execute As Func(Of Task), canExecute As Func(Of Boolean), onError As Action(Of Exception))
        _Execute = execute
        _CanExecute = canExecute
        _OnError = onError
    End Sub

    Sub New(execute As Func(Of Task), canExecute As Func(Of Boolean), onError As Action(Of Exception), onFinish As Action)
        _Execute = execute
        _CanExecute = canExecute
        _OnError = onError
        _OnFinish = onFinish
    End Sub

    Sub New(execute As Func(Of Task))
        _Execute = execute
    End Sub

    Public Async Sub Execute(parameter As Object) Implements ICommand.Execute
        Await ExecuteAsync()
    End Sub

    Public Async Function ExecuteAsync() As Task Implements IAsyncCommand.ExecuteAsync
        If CanExecute() Then
            Try
                _IsExecuting = True
                Await Task.Run(Sub() _Execute.Invoke)
            Catch ex As Exception
                If _OnError Is Nothing Then
                    Throw ex
                Else
                    _OnError(ex)
                End If
            Finally
                _OnFinish?.Invoke()
                _IsExecuting = False
            End Try
        End If
        RaiseCanExecuteChanged()
    End Function

    Public Function CanExecute() As Boolean Implements IAsyncCommand.CanExecute
        Return Not _IsExecuting AndAlso _CanExecute?.Invoke()
    End Function

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return CanExecute()
    End Function

    Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, New EventArgs)
    End Sub
End Class

Public Interface IAsyncCommand
    Inherits ICommand

    Function ExecuteAsync() As Task
    Overloads Function CanExecute() As Boolean
End Interface
