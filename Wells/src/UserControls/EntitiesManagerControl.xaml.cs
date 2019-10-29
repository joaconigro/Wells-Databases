using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wells.BaseView;

namespace Wells.View.UserControls
{
    /// <summary>
    /// Interaction logic for EntitiesManagerControl.xaml
    /// </summary>
    public partial class EntitiesManagerControl : UserControl
    {
        
        public EntitiesManagerControl()
        {
            InitializeComponent();
            //DataContext = this;
        }

        public static readonly DependencyProperty ObjectsSourceProperty = DependencyProperty.Register(nameof(ObjectsSource),
           typeof(object), typeof(EntitiesManagerControl), new PropertyMetadata(null));

        public static readonly DependencyProperty NewEntityCommandProperty = DependencyProperty.Register(nameof(NewEntityCommand),
           typeof(ICommand), typeof(EntitiesManagerControl), new PropertyMetadata(new RelayCommand(obj => { })));

        public static readonly DependencyProperty EditEntityCommandProperty = DependencyProperty.Register(nameof(EditEntityCommand),
           typeof(ICommand), typeof(EntitiesManagerControl), new PropertyMetadata(new RelayCommand(obj => { })));

        public static readonly DependencyProperty DeleteEntityCommandProperty = DependencyProperty.Register(nameof(DeleteEntityCommand),
           typeof(ICommand), typeof(EntitiesManagerControl), new PropertyMetadata(new RelayCommand(obj => { })));

        public static readonly DependencyProperty EditEntityVisibilityProperty = DependencyProperty.Register(nameof(EditEntityVisibility),
          typeof(Visibility), typeof(EntitiesManagerControl), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty NewEntityVisibilityProperty = DependencyProperty.Register(nameof(NewEntityVisibility),
           typeof(Visibility), typeof(EntitiesManagerControl), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty NewTextProperty = DependencyProperty.Register(nameof(NewText),
           typeof(string), typeof(EntitiesManagerControl), new PropertyMetadata("Nuevo..."));

        public ICommand NewEntityCommand { get => (ICommand)GetValue(NewEntityCommandProperty); set => SetValue(NewEntityCommandProperty, value); }
        public ICommand EditEntityCommand { get => (ICommand)GetValue(EditEntityCommandProperty); set => SetValue(EditEntityCommandProperty, value); }
        public ICommand DeleteEntityCommand { get => (ICommand)GetValue(DeleteEntityCommandProperty); set => SetValue(DeleteEntityCommandProperty, value); }
        public Visibility NewEntityVisibility { get => (Visibility)GetValue(NewEntityVisibilityProperty); set => SetValue(NewEntityVisibilityProperty, value); }
        public Visibility EditEntityVisibility { get => (Visibility)GetValue(EditEntityVisibilityProperty); set => SetValue(EditEntityVisibilityProperty, value); }
        public object ObjectsSource { get => GetValue(ObjectsSourceProperty); set => SetValue(ObjectsSourceProperty, value); }
        public object SelectedEntity { get; set; }
        public string NewText { get => (string)GetValue(ObjectsSourceProperty); set => SetValue(ObjectsSourceProperty, value); }
    }
}
