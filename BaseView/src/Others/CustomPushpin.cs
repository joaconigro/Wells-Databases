using Microsoft.Maps.MapControl.WPF;
using System.Windows;
using System.Windows.Controls;
using Wells.Model;

namespace Wells.BaseView
{
    public class CustomPushpin : Pushpin
    {
        public CustomPushpin(ControlTemplate template)
        {
            Template = template;
            ShowLabel = true;
        }

        public CustomPushpin(Well well, ControlTemplate template) : this(template)
        {
            Name = well.Name;
            Label = well.Name;
            Location = new Location(well.Latitude, well.Longitude);
        }

        public CustomPushpin(string name, double latitude, double longitude, ControlTemplate template) : this(template)
        {
            Name = name;
            Label = name;
            Location = new Location(latitude, longitude);
        }

        public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }
        public bool ShowLabel { get => (bool)GetValue(ShowLabelProperty); set => SetValue(ShowLabelProperty, value); }
        public bool IsSelected { get => (bool)GetValue(IsSelectedProperty); set => SetValue(IsSelectedProperty, value); }

        public double Value { get; set; }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label),
          typeof(string), typeof(CustomPushpin), new PropertyMetadata(""));

        public static readonly DependencyProperty ShowLabelProperty = DependencyProperty.Register(nameof(ShowLabel),
         typeof(bool), typeof(CustomPushpin), new PropertyMetadata(true));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected),
         typeof(bool), typeof(CustomPushpin), new PropertyMetadata(false));
    }
}
