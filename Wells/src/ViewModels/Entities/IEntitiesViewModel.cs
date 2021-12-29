using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wells.View.ViewModels
{
    public interface IEntitiesViewModel
    {
        bool IsNewCommandEnabled { get; }
        bool IsEditCommandEnabled { get; }
        ICommand NewEntityCommand { get; }
        ICommand EditEntityCommand { get; }
        ICommand RemoveEntityCommand { get; }
        ICommand ImportEntitiesCommand { get; }
        ICommand ExportEntitiesCommand { get; }
        ContextMenu GetContextMenu();

        void SetSelectedItems(IEnumerable<object> entities);
        void RemoveEventHandlers();
    }
}
