Imports System.Windows
Imports System.Windows.Controls

Public Class MenuItemExtensions
    Inherits DependencyObject

    Public Shared ElementToGroupNames As New Dictionary(Of MenuItem, String)

    Public Shared ReadOnly GroupNameProperty As DependencyProperty =
           DependencyProperty.RegisterAttached("GroupName",
                                        GetType(String),
                                        GetType(MenuItemExtensions),
                                        New PropertyMetadata(String.Empty, AddressOf OnGroupNameChanged))

    Public Shared Sub SetGroupName(element As MenuItem, value As String)
        element.SetValue(GroupNameProperty, value)
    End Sub

    Public Shared Function GetGroupName(element As MenuItem) As String
        Return element.GetValue(GroupNameProperty).ToString()
    End Function

    Private Shared Sub OnGroupNameChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        'Add an entry to the group name collection
        Dim menuItem = CType(d, MenuItem)

        If menuItem IsNot Nothing Then
            Dim newGroupName = e.NewValue.ToString()
            Dim oldGroupName = e.OldValue.ToString()
            If String.IsNullOrEmpty(newGroupName) Then
                'Removing the toggle button from grouping
                RemoveCheckboxFromGrouping(menuItem)
            Else
                'Switching to a New group
                If newGroupName <> oldGroupName Then
                    If Not String.IsNullOrEmpty(oldGroupName) Then
                        'Remove the old group mapping
                        RemoveCheckboxFromGrouping(menuItem)
                    End If
                    ElementToGroupNames.Add(menuItem, e.NewValue.ToString())
                    AddHandler menuItem.Click, AddressOf MenuItemClicked
                End If
            End If
        End If
    End Sub

    Private Shared Sub RemoveCheckboxFromGrouping(checkBox As MenuItem)
        ElementToGroupNames.Remove(checkBox)
        RemoveHandler checkBox.Click, AddressOf MenuItemClicked
    End Sub

    Shared Sub MenuItemClicked(sender As Object, e As RoutedEventArgs)
        Dim menuItem = CType(sender, MenuItem)
        menuItem.IsChecked = True

        For Each item In ElementToGroupNames
            If item.Key IsNot menuItem AndAlso item.Value = GetGroupName(menuItem) Then
                item.Key.IsChecked = False
            End If
        Next
    End Sub

End Class

