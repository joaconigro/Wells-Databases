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
                    App.Settings.SetConnectionString(param.ToString());
                    CloseModalViewCommand.Execute(true);
                }, (obj) => App.Settings != null, OnError);
            }
        }
    }
}
