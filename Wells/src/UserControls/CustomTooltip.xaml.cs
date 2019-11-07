using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wells.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomTooltip.xaml
    /// </summary>
    public partial class CustomTooltip : UserControl
    {
        public CustomTooltip()
        {
            InitializeComponent();
            DataContext = this;
        }

        public CustomTooltip(string title) : this() { Title = title; }
        public CustomTooltip(string title, string info) : this(title) { Info = info; }


        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
          typeof(string), typeof(CustomTooltip), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty InfoProperty = DependencyProperty.Register(nameof(Info),
          typeof(string), typeof(CustomTooltip), new PropertyMetadata(string.Empty));

        public bool ShowInfo => !string.IsNullOrEmpty(Info);

        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public string Info { get => (string)GetValue(InfoProperty); set => SetValue(InfoProperty, value); }
    }
}
