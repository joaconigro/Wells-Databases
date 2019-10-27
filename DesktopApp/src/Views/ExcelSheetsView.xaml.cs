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
using System.Windows.Shapes;

namespace Wells.View
{
    /// <summary>
    /// Interaction logic for ExcelSheetsView.xaml
    /// </summary>
    public partial class ExcelSheetsView : Window
    {
        public List<string> Sheets { get; set; }
          public  int SelectedSheet { get; set; }

        public ExcelSheetsView(List<string> sheets)
        {
            InitializeComponent();
            SelectedSheet = 0;
            Sheets = sheets;
            DataContext = this;
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
