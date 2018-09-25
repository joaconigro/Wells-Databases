Imports System.ComponentModel
Imports Wells.Base

Public MustInherit Class BaseViewModel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub NotifyPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Protected MustOverride Sub ShowErrorMessage(message As String)

    Protected Sub OnError(ex As Exception)
        Try
            ExceptionHandler.Handle(ex)
        Catch exe As Exception
            ShowErrorMessage(ExceptionHandler.GetAllMessages(ex))
        End Try
    End Sub

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
