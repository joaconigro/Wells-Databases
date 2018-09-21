Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls

Namespace Extensions

    ''' <summary>
    ''' Using this behavior on a DataGrid will ensure to display only columns with "Browsable Attributes"
    ''' </summary>
    Public Class DataGridBehavior

        Public Shared ReadOnly UseBrowsableAttributeOnColumnProperty As DependencyProperty =
            DependencyProperty.RegisterAttached("UseBrowsableAttributeOnColumn",
            GetType(Boolean),
            GetType(DataGridBehavior),
            New UIPropertyMetadata(False, AddressOf UseBrowsableAttributeOnColumnChanged))


        Public Shared Function GetUseBrowsableAttributeOnColumn(obj As DependencyObject) As Boolean
            Return CType(obj.GetValue(UseBrowsableAttributeOnColumnProperty), Boolean)
        End Function


        Public Shared Sub SetUseBrowsableAttributeOnColumn(obj As DependencyObject, val As Boolean)
            obj.SetValue(UseBrowsableAttributeOnColumnProperty, val)
        End Sub

        Private Shared Sub UseBrowsableAttributeOnColumnChanged(obj As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim dataGrid = CType(obj, DataGrid)
            If dataGrid IsNot Nothing Then
                If CType(e.NewValue, Boolean) Then
                    AddHandler dataGrid.AutoGeneratingColumn, AddressOf DataGridOnAutoGeneratingColumn
                Else
                    RemoveHandler dataGrid.AutoGeneratingColumn, AddressOf DataGridOnAutoGeneratingColumn
                End If
            End If
        End Sub

        Private Shared Sub DataGridOnAutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs)
            Dim propDesc = CType(e.PropertyDescriptor, PropertyDescriptor)

            If e.PropertyType Is GetType(DateTime) Then
                CType(e.Column, DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"
                Dim style = New Style(GetType(DataGridCell), e.Column.CellStyle)
                Dim setter = New Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Right)
                style.Setters.Add(setter)
                e.Column.CellStyle = style
            ElseIf e.PropertyType Is GetType(Double) Then
                CType(e.Column, DataGridTextColumn).Binding.StringFormat = "N3"
                Dim style = New Style(GetType(DataGridCell), e.Column.CellStyle)
                Dim setter = New Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Right)
                style.Setters.Add(setter)
                e.Column.CellStyle = style
            End If

            If propDesc IsNot Nothing Then
                For Each att In propDesc.Attributes
                    If TypeOf att Is BrowsableAttribute Then
                        Dim browsableAttribute = CType(att, BrowsableAttribute)

                        If browsableAttribute IsNot Nothing Then
                            e.Cancel = Not browsableAttribute.Browsable
                        End If
                    End If

                    If TypeOf att Is DisplayNameAttribute Then
                        Dim displayName = CType(att, DisplayNameAttribute)
                        If displayName IsNot Nothing Then
                            e.Column.Header = displayName.DisplayName
                        End If
                    End If
                Next
            End If
        End Sub

    End Class

End Namespace