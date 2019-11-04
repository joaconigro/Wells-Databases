using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wells.BaseModel.Attributes;
using Wells.BaseView.Converters;

namespace Wells.BaseView.Behaviors
{
    public class DataGridBehavior
    {

        public static readonly DependencyProperty UseBrowsableAttributeOnColumnProperty =
             DependencyProperty.RegisterAttached("UseBrowsableAttributeOnColumn",
            typeof(bool),
            typeof(DataGridBehavior),
            new UIPropertyMetadata(false, UseBrowsableAttributeOnColumnChanged));

        public static bool GetUseBrowsableAttributeOnColumn(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseBrowsableAttributeOnColumnProperty);
        }

        public static void SetUseBrowsableAttributeOnColumn(DependencyObject obj, bool val)
        {
            obj.SetValue(UseBrowsableAttributeOnColumnProperty, val);
        }


        private static void UseBrowsableAttributeOnColumnChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)obj;
            if (dataGrid != null)
            {
                if ((bool)e.NewValue)
                {
                    dataGrid.AutoGeneratingColumn += DataGridOnAutoGeneratingColumn;
                }
                else
                {
                    dataGrid.AutoGeneratingColumn -= DataGridOnAutoGeneratingColumn;
                }
            }
        }


        private static void DataGridOnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var propDesc = (PropertyDescriptor)e.PropertyDescriptor;

            bool changeStyle = false;
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yy";
                changeStyle = true;
            }
            else if (e.PropertyType == typeof(double))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "N3";
                changeStyle = true;
            }
            else if (e.PropertyType == typeof(int))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "N0";
                changeStyle = true;
            }

            if (changeStyle)
            {
                var style = new Style(typeof(TextBlock), (e.Column as DataGridTextColumn).ElementStyle);
                var setter = new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                style.Setters.Add(setter);
                (e.Column as DataGridTextColumn).ElementStyle = style;
            }

            if (propDesc != null)
            {
                foreach (var att in propDesc.Attributes)
                {
                    if (att.GetType() == typeof(BrowsableAttribute))
                    {
                        if (att is BrowsableAttribute browsableAttribute)
                        {
                            if (!browsableAttribute.Browsable)
                            {
                                e.Cancel = !browsableAttribute.Browsable;
                                return;
                            }
                        }
                    }


                    if (att.GetType() == typeof(DisplayNameAttribute))
                    {
                        if (att is DisplayNameAttribute displayName)
                        {
                            e.Column.Header = displayName.DisplayName;
                        }
                    }


                    if ((sender as DataGrid).Columns.Count > 0)
                    {
                        if (att.GetType() == typeof(SortIndexAttribute))
                        {
                            if (att is SortIndexAttribute sortIndex)
                            {
                                e.Column.DisplayIndex = (int)sortIndex.Index;
                            }
                        }
                    }
                }
            }


            if (e.PropertyType.IsEnum)
            {
                (e.Column as DataGridComboBoxColumn).ItemsSource = Base.Common.EnumDescriptionsToList(e.PropertyType);
                var propPath = (e.Column as DataGridComboBoxColumn).SortMemberPath;
                (e.Column as DataGridComboBoxColumn).SelectedItemBinding = new Binding()
                {
                    Path = new PropertyPath(propPath),
                    Converter = new EnumValueConverter(),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
            }

        }
    }
}
