Imports System.Windows

Public Class CheckedListItem
    Inherits DependencyObject

    Property Text As String
        Get
            Return GetValue(TextProperty)
        End Get
        Set(value As String)
            SetValue(TextProperty, value)
        End Set
    End Property

    Property IsChecked As Boolean
        Get
            Return GetValue(IsCheckedProperty)
        End Get
        Set(value As Boolean)
            SetValue(IsCheckedProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register(
        NameOf(Text),
        GetType(String),
        GetType(CheckedListItem))

    Public Shared ReadOnly IsCheckedProperty As DependencyProperty = DependencyProperty.Register(
        NameOf(IsChecked),
        GetType(Boolean),
        GetType(CheckedListItem))

    Sub New(text As String)
        Me.Text = text
    End Sub

End Class
