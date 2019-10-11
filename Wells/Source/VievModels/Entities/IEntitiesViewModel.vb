Public Interface IEntitiesViewModel

    ReadOnly Property IsNewCommandEnabled As Boolean
    ReadOnly Property IsRemoveCommandEnabled As Boolean

    ReadOnly Property NewEntityCommand As ICommand
    ReadOnly Property ImportEntitiesCommand As ICommand
    ReadOnly Property EditEntityCommand As ICommand
    ReadOnly Property RemoveEntityCommand As ICommand

    Function GetContextMenu() As ContextMenu
    Sub SetSelectedEntities(entities As IEnumerable(Of Object))

End Interface
