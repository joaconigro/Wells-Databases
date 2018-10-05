Imports System.ComponentModel

Public Class CheckedListItem(Of T)
    Implements INotifyPropertyChanged

    Private _isChecked As Boolean
    Private _item As T

    Sub New()

    End Sub

    Sub New(item As T, isChecked As Boolean)
        _item = item
        _isChecked = isChecked
    End Sub

    Property Item As T
        Get
            Return _item
        End Get
        Set(value As T)
            _item = value
            NotifyPropertyChanged(NameOf(Item))
        End Set
    End Property

    Property IsChecked As Boolean
        Get
            Return _isChecked
        End Get
        Set(value As Boolean)
            _isChecked = value
            NotifyPropertyChanged(NameOf(IsChecked))
            RaiseEvent CheckedChanged(Item, _isChecked)
        End Set
    End Property

    Private Sub NotifyPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Event CheckedChanged(sender As T, e As Boolean)

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
