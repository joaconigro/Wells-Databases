Imports System.Collections.ObjectModel

Public Class FilterCollection(Of T)
    Inherits ObservableCollection(Of BaseFilter(Of T))

    Protected _PropertyName As String

    Event FiltersChanged()

    Sub RaiseCollectionChanged()
        RaiseEvent FiltersChanged()
    End Sub

    Function Apply(list As IQueryable(Of T)) As IQueryable(Of T)
        For Each f In Me
            If f.IsEnabled Then
                list = f.Apply(list)
            End If
        Next
        Return list
    End Function
End Class
