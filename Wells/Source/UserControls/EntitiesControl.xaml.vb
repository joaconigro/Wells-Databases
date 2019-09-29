Imports System.Reflection
Imports System.Windows
Imports Wells.ViewBase

Public Class EntitiesControl
    Implements IEntitiesControl

    Public ReadOnly Property Window As Window Implements IEntitiesControl.Window

    Public ReadOnly Property MainWindow As IMainWindowView Implements IEntitiesControl.MainWindow

    Property RowContextMenu As ContextMenu
        Get
            Return GetValue(RowContextMenuProperty)
        End Get
        Set(value As ContextMenu)
            SetValue(RowContextMenuProperty, value)
        End Set
    End Property

    Public Shared ReadOnly RowContextMenuProperty As DependencyProperty =
        DependencyProperty.Register(NameOf(RowContextMenu),
                                    GetType(ContextMenu),
                                    GetType(EntitiesControl),
                                    New PropertyMetadata(Nothing))

    Sub SetViewModel(vm As IEntitiesViewModel)
        _Window = Window.GetWindow(Me)
        _MainWindow = CType(Window, IMainWindowView)
        DataContext = vm
        CType(vm, BaseViewModel).SetView(Me)
        RowContextMenu = vm.GetContextMenu()
    End Sub


#Region "IView implementation"
    Public Sub ShowErrorMessageBox(message As String) Implements IView.ShowErrorMessageBox
        SharedBaseView.ShowErrorMessageBox(Window, message)
    End Sub

    Public Sub ShowOkOnkyMessageBox(message As String, title As String) Implements IView.ShowOkOnkyMessageBox
        SharedBaseView.ShowOkOnkyMessageBox(Window, message, title)
    End Sub

    Public Sub CloseView(dialogResult As Boolean?) Implements IView.CloseView

    End Sub

    Public Sub CloseView() Implements IView.CloseView

    End Sub

    Public Function OpenFileDialog(filter As String, title As String, Optional initialDirectory As String = "") As String Implements IView.OpenFileDialog
        Return SharedBaseView.OpenFileDialog(filter, title, initialDirectory)
    End Function

    Public Function ShowMessageBox(message As String, title As String) As Boolean Implements IView.ShowMessageBox
        Return SharedBaseView.ShowMessageBox(Window, message, title)
    End Function

    Public Function SaveFileDialog(filter As String, title As String, Optional filename As String = "", Optional initialDirectory As String = "") As String Implements IView.SaveFileDialog
        Return SharedBaseView.SaveFileDialog(filter, title, filename, initialDirectory)
    End Function

    Public Function ShowInputBox(prompt As String, Optional title As String = "", Optional defaultResponse As String = "") As String Implements IView.ShowInputBox
        Return SharedBaseView.ShowInputBox(prompt, title, defaultResponse)
    End Function

    Public Function ShowFolderSelectionDialog() As String Implements IView.ShowFolderSelectionDialog
        Return SharedBaseView.ShowFolderSelectionDialog()
    End Function
#End Region

    Function OpenAddOrEditFilterDialog(vm As FilterViewModel) As Boolean Implements IEntitiesControl.OpenAddOrEditFilterDialog
        Dim diag As New FilterEditingView(vm) With {.Owner = Window}
        Return diag.ShowDialog()
    End Function

    Public Sub ForceListBoxItemsRefresh() Implements IEntitiesControl.ForceListBoxItemsRefresh
        FiltersListBox.Items.Refresh()
    End Sub

    Private Sub OnRowEditing(sender As Object, e As DataGridRowEditEndingEventArgs)
        Dim dg = CType(sender, DataGrid)
        If dg.SelectedItem IsNot Nothing Then
            RemoveHandler dg.RowEditEnding, AddressOf OnRowEditing
            dg.CommitEdit()
            dg.Items.Refresh()
            AddHandler dg.RowEditEnding, AddressOf OnRowEditing
        End If
    End Sub

    Public Sub ForceDataGridRefresh() Implements IEntitiesControl.ForceDataGridRefresh
        OnRowEditing(EntitiesDataGrid, Nothing)
    End Sub

End Class

Public Interface IEntitiesControl
    Inherits IView
    ReadOnly Property Window As Window
    ReadOnly Property MainWindow As IMainWindowView
    Function OpenAddOrEditFilterDialog(vm As FilterViewModel) As Boolean
    Sub ForceListBoxItemsRefresh()
    Sub ForceDataGridRefresh()
End Interface
