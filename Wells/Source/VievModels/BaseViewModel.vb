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
End Class
