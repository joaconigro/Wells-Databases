using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wells.BaseView.ViewInterfaces;

namespace Wells.BaseView
{
    public class SharedBaseView : Window
    {
        private SharedBaseView() { }

        public static void ShowErrorMessageBox(Window owner, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(owner, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }

        public static void ShowErrorMessageBox(IView view, string message)
        {
            ShowErrorMessageBox((Window)view, message);
        }

        public static string OpenFileDialog(string filter, string title)
        {
            return OpenFileDialog(filter, title, "");
        }

        public static string OpenFileDialog(string filter, string title, string initialDirectory)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var ofd = new Microsoft.Win32.OpenFileDialog { Filter = filter, Title = title, InitialDirectory = initialDirectory };
                if (ofd.ShowDialog() == true) { return ofd.FileName; }
                return null;
            });
        }

        public static List<string> OpenMultipleFileDialog(string filter, string title, string initialDirectory)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var ofd = new Microsoft.Win32.OpenFileDialog { Filter = filter, Title = title, InitialDirectory = initialDirectory, Multiselect = true };
                if (ofd.ShowDialog() == true) { return ofd.FileNames.ToList(); }
                return null;
            });
        }

        public static List<string> OpenMultipleFileDialog(string filter, string title)
        {
            return OpenMultipleFileDialog(filter, title, string.Empty);
        }

        public static string SaveFileDialog(string filter, string title, string filename, string initialDirectory)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var sfd = new Microsoft.Win32.SaveFileDialog { Filter = filter, Title = title, FileName = filename, InitialDirectory = initialDirectory };
                if (sfd.ShowDialog() == true) { return sfd.FileName; }
                return null;
            });
        }

        public static string SaveFileDialog(string filter, string title)
        {
            return SaveFileDialog(filter, title, string.Empty, string.Empty);
        }

        public static string SaveFileDialog(string filter, string title, string filename)
        {
            return SaveFileDialog(filter, title, filename, string.Empty);
        }

        public static bool ShowYesNoMessageBox(Window owner, string message, string title)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                return MessageBox.Show(owner, message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            });
        }

        public static bool ShowYesNoMessageBox(IView view, string message, string title)
        {
            return ShowYesNoMessageBox((Window)view, message, title);
        }

        public static void ShowOkOnkyMessageBox(Window owner, string message, string title)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public static void ShowOkOnkyMessageBox(IView view, string message, string title)
        {
            ShowOkOnkyMessageBox((Window)view, message, title);
        }

        public static string ShowInputBox(Window owner, string message, string title)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var inputBox = new InputView() { WindowTitle = title, Message = message, Owner = owner };
                if (inputBox.ShowDialog() == true)
                {
                    return inputBox.Input;
                }
                return null;
            });
        }

        public static string ShowInputBox(IView owner, string message, string title)
        {
            return ShowInputBox((Window)owner, message, title);
        }

        public static Color ShowColorDialog(System.Drawing.Color selectedColor)
        {
            using (var diag = new Cyotek.Windows.Forms.ColorPickerDialog { Color = selectedColor })
            {
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return Color.FromArgb(diag.Color.A, diag.Color.R, diag.Color.G, diag.Color.B);
                }
            }
            return Color.FromArgb(selectedColor.A, selectedColor.R, selectedColor.G, selectedColor.B);
        }

        public static Color GetRandomColor(Random random)
        {
            var names = (IList)Enum.GetValues(typeof(System.Drawing.KnownColor));
            var randomColorName = (System.Drawing.KnownColor)names[random.Next(names.Count)];
            var randomColor = System.Drawing.Color.FromKnownColor(randomColorName);
            return Color.FromArgb(randomColor.A, randomColor.R, randomColor.G, randomColor.B);
        }

        public static void CaptureScreen(string imageFilename, Visual target, double dpiX, double dpiY)
        {
            if (target == null) { return; }

            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            var rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                         (int)(bounds.Height * dpiY / 96.0),
                                         dpiX, dpiY, PixelFormats.Pbgra32);

            var dv = new DrawingVisual();
            using (var ctx = dv.RenderOpen())
            {
                var vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            var frame = BitmapFrame.Create(rtb);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);

            using var stream = System.IO.File.Create(imageFilename);
            encoder.Save(stream);
        }
    }
}
