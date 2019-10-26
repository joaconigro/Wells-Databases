using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wells.CoreView;

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
        }

        public static readonly DependencyProperty ObjectsSourceProperty = DependencyProperty.Register(nameof(ObjectsSource),
           typeof(object), typeof(EntitiesManagerControl), new PropertyMetadata(null));

        public static readonly DependencyProperty NewEntityCommandProperty = DependencyProperty.Register(nameof(NewEntityCommand),
           typeof(ICommand), typeof(EntitiesManagerControl), new PropertyMetadata(new RelayCommand(obj => { })));

        public static readonly DependencyProperty EditEntityCommandProperty = DependencyProperty.Register(nameof(EditEntityCommand),
           typeof(ICommand), typeof(EntitiesManagerControl), new PropertyMetadata(new RelayCommand(obj => { })));

        public static readonly DependencyProperty DeleteEntityCommandProperty = DependencyProperty.Register(nameof(DeleteEntityCommand),
           typeof(ICommand), typeof(EntitiesManagerControl), new PropertyMetadata(new RelayCommand(obj => { })));


        ICommand NewEntityCommand { get => (ICommand)GetValue(NewEntityCommandProperty); set => SetValue(NewEntityCommandProperty, value); }

        ICommand EditEntityCommand { get => (ICommand)GetValue(EditEntityCommandProperty); set => SetValue(EditEntityCommandProperty, value); }
        ICommand DeleteEntityCommand { get => (ICommand)GetValue(DeleteEntityCommandProperty); set => SetValue(DeleteEntityCommandProperty, value); }

        public object ObjectsSource { get => GetValue(ObjectsSourceProperty); set => SetValue(ObjectsSourceProperty, value); }
        public object SelectedEntity { get; set; }

    }
}
