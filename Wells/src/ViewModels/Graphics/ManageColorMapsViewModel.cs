using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;
using Wells.Resources;
using Wells.View.Graphics;

namespace Wells.View.ViewModels
{
    public class ManageColorMapsViewModel : BaseViewModel
    {
        private Gradient selectedGradient;
        private string gradientName;
        private LinearGradientBrush rectangleFill;
        private IManageColorMapsView dialog;

        public ObservableCollection<Gradient> Gradients { get; private set; }
        public Gradient SelectedGradient { get => selectedGradient; set { SetValue(ref selectedGradient, value); UpdateGradient(); } }
        public string GradientName { get => gradientName; set { SetValue(ref gradientName, value); UpdateGradientName(); } }
        public LinearGradientBrush RectangleFill { get => rectangleFill; set { SetValue(ref rectangleFill, value); } }
        public bool EnablePropertiesPanel => SelectedGradient != null;


        public void UpdateGradient()
        {
            if (SelectedGradient != null)
            {
                gradientName = SelectedGradient.Name;
                RectangleFill = SelectedGradient.LinearGradient;
            }
            NotifyPropertyChanged(nameof(GradientName));
            NotifyPropertyChanged(nameof(EnablePropertiesPanel));
            dialog.CreateSliders();
        }

        public void UpdateGradientName()
        {
            var list = Gradients.ToList();
            if (SelectedGradient != null)
            {
                SelectedGradient.Name = gradientName;
                Gradients.Clear();
                foreach (var g in list)
                {
                    Gradients.Add(g);
                }
                SelectedGradient = Gradients.First(g => g.Name == gradientName);
            }
        }

        public ManageColorMapsViewModel(IView view) : base(view)
        {
            Gradients = new ObservableCollection<Gradient>(ReadGradients());
            Initialize();
        }

        protected override void OnSetView(IView view)
        {
            dialog = (IManageColorMapsView)view;
        }

        protected override void SetCommandUpdates()
        {
            Add(nameof(SelectedGradient), new List<ICommand> { RemoveGradientCommand, InvertGradientCommand, ExportGradientCommand });
        }

        public static List<Gradient> ReadGradients()
        {
            var filename = Path.Combine(AppSettings.SettingsDirectory, "Gradients.wgr");

            var gradients = new List<Gradient>();
            if (File.Exists(filename))
            {
                var serializer = new XmlSerializer(typeof(List<Gradient>));
                using var reader = new StreamReader(filename);
                gradients = (List<Gradient>)serializer.Deserialize(reader);
            }

            foreach (var g in gradients)
            {
                g.DeserializeGradient();
            }
            return gradients;
        }

        public void SaveGradients()
        {
            var filename = Path.Combine(AppSettings.SettingsDirectory, "Gradients.wgr");

            foreach (var g in Gradients)
            {
                g.SerializeGradient();
            }

            var serializer = new XmlSerializer(typeof(List<Gradient>));
            using var writer = new StreamWriter(filename);
            serializer.Serialize(writer, Gradients.ToList());
        }

        public ICommand InvertGradientCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    SelectedGradient.Invert();
                    UpdateGradient();
                }, (obj) => SelectedGradient != null, OnError);
            }
        }

        public ICommand NewGradientCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var grad = new Gradient();
                    Gradients.Add(grad);
                    SelectedGradient = grad;
                }, (obj) => true, OnError);
            }
        }

        public ICommand ImportGradientCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var filename = SharedBaseView.OpenFileDialog("Tablas de colores (*.wgr)|*.wgr", "Importar tabla...");
                    if (!string.IsNullOrEmpty(filename))
                    {
                        var serializer = new XmlSerializer(typeof(Gradient));
                        using var reader = new StreamReader(filename);
                        var grad = (Gradient)serializer.Deserialize(reader);
                        grad.DeserializeGradient();
                        Gradients.Add(grad);
                        SelectedGradient = grad;
                    }                  
                }, (obj) => true, OnError);
            }
        }

        public ICommand ExportGradientCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var filename = SharedBaseView.SaveFileDialog("Tablas de colores (*.wgr)|*.wgr", "Exportar tabla...", SelectedGradient.Name);
                    if (!string.IsNullOrEmpty(filename))
                    {
                        var serializer = new XmlSerializer(typeof(Gradient));
                        using var writer = new StreamWriter(filename);
                        serializer.Serialize(writer, SelectedGradient);
                    }
                }, (obj) => SelectedGradient != null, OnError);
            }
        }

        public ICommand RemoveGradientCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    if (SharedBaseView.ShowYesNoMessageBox(View, "¿Está seguro de eliminar esta tabla de colores?", "Eliminar"))
                    {
                        Gradients.Remove(SelectedGradient);
                        SelectedGradient = null;
                        UpdateGradient();
                    }
                }, (obj) => SelectedGradient != null, OnError);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    SaveGradients();
                    CloseModalViewCommand.Execute(true);
                }, (obj) => true, OnError);
            }
        }
    }
}
