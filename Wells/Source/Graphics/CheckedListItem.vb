Imports System.ComponentModel

Public Class CheckedListItem(Of T)
    Implements INotifyPropertyChanged

    Private _isChecked As Boolean
    Private _name As T

    Sub New()

    End Sub

    Sub New(item As T, isChecked As Boolean)
        _name = item
        _isChecked = isChecked
    End Sub

    Property Name As T
        Get
            Return _name
        End Get
        Set(value As T)
            _name = value
            NotifyPropertyChanged(NameOf(Name))
        End Set
    End Property

    Property IsChecked As Boolean
        Get
            Return _isChecked
        End Get
        Set(value As Boolean)
            _isChecked = value
            NotifyPropertyChanged(NameOf(IsChecked))
            RaiseEvent CheckedChanged(Name, _isChecked)
        End Set
    End Property

    Private Sub NotifyPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Event CheckedChanged(sender As T, e As Boolean)

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Overrides Function ToString() As String
        Return Name.ToString()
    End Function
End Class
