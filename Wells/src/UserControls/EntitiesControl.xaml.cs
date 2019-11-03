using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.View.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Wells.BaseView.ViewModel;

namespace Wells.View.UserControls
{
    /// <summary>
    /// Interaction logic for EntitiesControl.xaml
    /// </summary>
    public partial class EntitiesControl : UserControl, IEntitiesControl
    {
        Window _Window;
        IMainWindow _MainWindow;
        IEntitiesViewModel _ViewModel;

        public EntitiesControl()
        {
            InitializeComponent();
        }

        public Window Window => _Window;

        public IMainWindow MainWindow => _MainWindow;

        public ContextMenu RowContextMenu { get => (ContextMenu)GetValue(RowContextMenuProperty); set { SetValue(RowContextMenuProperty, value); } }

        public static readonly DependencyProperty RowContextMenuProperty = DependencyProperty.Register(nameof(RowContextMenu),
            typeof(ContextMenu), typeof(EntitiesControl), new PropertyMetadata(null));


        #region IView
        public void CloseView(bool? dialogResult) { }

        public void CloseView() { }

        public string OpenFileDialog(string filter, string title, string initialDirectory)
        {
            return SharedBaseView.OpenFileDialog(filter, title, initialDirectory);
        }

        public string OpenFileDialog(string filter, string title)
        {
            return SharedBaseView.OpenFileDialog(filter, title);
        }

        public string SaveFileDialog(string filter, string title, string filename, string initialDirectory)
        {
            return SharedBaseView.SaveFileDialog(filter, title, filename, initialDirectory);
        }

        public string SaveFileDialog(string filter, string title)
        {
            return SharedBaseView.SaveFileDialog(filter, title);
        }

        public string SaveFileDialog(string filter, string title, string filename)
        {
            return SharedBaseView.SaveFileDialog(filter, title, filename);
        }


        public void ShowErrorMessageBox(string message)
        {
            SharedBaseView.ShowErrorMessageBox(Window, message);
        }

        public bool ShowYesNoMessageBox(string message, string title)
        {
            return SharedBaseView.ShowYesNoMessageBox(Window, message, title);
        }

        public void ShowOkOnkyMessageBox(string message, string title)
        {
            SharedBaseView.ShowOkOnkyMessageBox(Window, message, title);
        }
        #endregion

        void OnRowEditing(object sender, DataGridRowEditEndingEventArgs e)
        {
            var dg = (DataGrid)sender;
            if (dg.SelectedItem != null)
            {
                dg.RowEditEnding -= OnRowEditing;
                dg.CommitEdit();
                dg.Items.Refresh();
                dg.RowEditEnding += OnRowEditing;
            }
        }

        public void ForceDataGridRefresh()
        {
            OnRowEditing(EntitiesDataGrid, null);
        }

        public void ForceListBoxItemsRefresh()
        {
            FiltersListBox.Items.Refresh();
        }

        public bool ShowFilterDialog(FilterViewModel viewModel)
        {
            var diag = new FilterEditingView(viewModel) { Owner = Window };
            return (bool)(diag.ShowDialog());
        }

        public void SetViewModel(IEntitiesViewModel viewModel)
        {
            _Window = Window.GetWindow(this);
            _MainWindow = (IMainWindow)Window;
            DataContext = viewModel;
            _ViewModel = viewModel;
            (viewModel as BaseViewModel).SetView(this);
            RowContextMenu = viewModel.GetContextMenu();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = (from object o in (sender as DataGrid).SelectedItems
                         select o).ToList();
            _ViewModel.SetSelectedItems(items);
        }

        public void UpdateRowContextMenu()
        {
            RowContextMenu = _ViewModel.GetContextMenu();
        }
    }

    public interface IEntitiesControl : IView
    {
        Window Window { get; }
        IMainWindow MainWindow { get; }

        void SetViewModel(IEntitiesViewModel viewModel);
        bool ShowFilterDialog(FilterViewModel viewModel);
        void ForceListBoxItemsRefresh();
        void ForceDataGridRefresh();
        void UpdateRowContextMenu();
    }
}
