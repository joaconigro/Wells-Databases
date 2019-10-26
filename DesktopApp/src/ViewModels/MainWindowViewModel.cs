using Wells.CoreView.ViewInterfaces;
using Wells.CoreView.ViewModel;
using Wells.CorePersistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using Wells.CorePersistence;

namespace Wells.View.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        RepositoryWrapper _Repository;
        IMainWindow _MainWindow;
        private bool repositoryIsOpened;

        public bool RepositoryIsOpened => repositoryIsOpened;
       
        public MainWindowViewModel(IView view) : base(null)
        {
            SetView(view);
            InitializeContext();
            Initialize();
        }

        protected override void OnSetView(IView view)
        {
            _MainWindow = (IMainWindow)view;
        }

        void InitializeContext()
        {
            try
            {
                string connectionString;
#if DEBUG
                connectionString = Application.Current.Resources["SQLConnection"].ToString();
                RepositoryWrapper.Instantiate(connectionString);
#else
                connectionString = Application.Current.Resources["MySQLConnection"].ToString();
                context.Database.Migrate();
                RepositoryWrapper.Instantiate(connectionString);
#endif
                _Repository = RepositoryWrapper.Instance;
                repositoryIsOpened = _Repository != null;

            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            finally
            {
                NotifyPropertyChanged(nameof(RepositoryIsOpened));
            }
        }

        protected override void SetCommandUpdates() { }

        protected override void SetValidators() { }
    }
}
