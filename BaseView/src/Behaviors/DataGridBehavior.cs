﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wells.Base;
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
            if (Common.IsNumericType(e.PropertyType))
            {
                if (Common.IsIntegerNumericType(e.PropertyType))
                {
                    (e.Column as DataGridTextColumn).Binding.StringFormat = "N0";
                }
                else
                {
                    (e.Column as DataGridTextColumn).Binding.StringFormat = "N3";
                }
                changeStyle = true;
            }
         
            if (changeStyle)
            {
                ((e.Column as DataGridTextColumn).Binding as Binding).ConverterCulture = System.Globalization.CultureInfo.CurrentCulture;
                var style = new Style(typeof(TextBlock), (e.Column as DataGridTextColumn).ElementStyle);
                var setter = new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                style.Setters.Add(setter);
                (e.Column as DataGridTextColumn).ElementStyle = style;
            }

            bool thereAreColumns = (sender as DataGrid).Columns.Count > 0;

            if (propDesc != null)
            {
                foreach (var att in propDesc.Attributes)
                {
                    if (att is BrowsableAttribute browsableAttribute && !browsableAttribute.Browsable)
                    {
                        e.Cancel = !browsableAttribute.Browsable;
                        return;
                    }

                    if (att is DisplayNameAttribute displayName)
                    {
                        e.Column.Header = displayName.DisplayName;
                    }

                    if (thereAreColumns && att is SortIndexAttribute sortIndex)
                    {
                        e.Column.DisplayIndex = (int)sortIndex.Index;
                    }
                }
            }


            if (e.PropertyType.IsEnum)
            {
                (e.Column as DataGridComboBoxColumn).ItemsSource = Base.Common.EnumDescriptionsToList(e.PropertyType);
                var propPath = (e.Column as DataGridComboBoxColumn).SortMemberPath;
                (e.Column as DataGridComboBoxColumn).SelectedItemBinding = new Binding
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
