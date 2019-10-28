//using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wells.CoreView
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

        public static string OpenFileDialog(string filter, string title, string initialDirectory = "")
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = filter, Title = title, InitialDirectory = initialDirectory };
                if (ofd.ShowDialog() == true)
                    return ofd.FileName;
                return null;
            });
        }

        public static List<string> OpenMultipleFileDialog(string filter, string title, string initialDirectory = "")
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = filter, Title = title, InitialDirectory = initialDirectory, Multiselect = true };
                if (ofd.ShowDialog() == true)
                    return ofd.FileNames.ToList();
                return null;
            });
        }

        public static string SaveFileDialog(string filter, string title, string filename = "", string initialDirectory = "")
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var sfd = new Microsoft.Win32.SaveFileDialog() { Filter = filter, Title = title, FileName = filename, InitialDirectory = initialDirectory };
                if (sfd.ShowDialog() == true)
                    return sfd.FileName;
                return null;
            });
        }

        public static bool ShowYesNoMessageBox(Window owner, string message, string title)
        {
            return Application.Current.Dispatcher.Invoke(() =>
            {
                return MessageBox.Show(owner, message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            });
        }

        public static void ShowOkOnkyMessageBox(Window owner, string message, string title)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        //public static string ShowInputBox(string prompt, string title = "", string filename = "", string defaultResponse = "")
        //{
        //    return Interaction.InputBox(prompt, title, defaultResponse);
        //}

        //public static string ShowFolderSelectionDialog()
        //{
        //    using (var diag = new Forms.FolderBrowserDialog())
        //    {
        //        if (diag.ShowDialog() == Forms.DialogResult.OK)
        //            return diag.SelectedPath;
        //    }
        //    return null;
        //}




        public static void CaptureScreen(string imageFilename, Visual target, double dpiX, double dpiY)
        {
            if (target == null) return;

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

            using (var stream = System.IO.File.Create(imageFilename))
            {
                encoder.Save(stream);
            }
        }
    }
}
