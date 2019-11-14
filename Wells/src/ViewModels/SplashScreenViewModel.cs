using System.Windows.Input;
using Wells.BaseView;
using Wells.BaseView.ViewInterfaces;
using Wells.BaseView.ViewModel;

namespace Wells.View.ViewModels
{
    public class SplashScreenViewModel : BaseViewModel
    {

        public SplashScreenViewModel(IView view) : base(view)
        {
            Initialize();
        }

        public ICommand SelectConnectionStringCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var conn = App.Settings.ConnectionStrings[param.ToString()];
                    App.Settings.CurrentConnectionString = conn;
                    CloseModalViewCommand.Execute(true);
                }, (obj) => App.Settings != null, OnError);
            }
        }
    }
}
