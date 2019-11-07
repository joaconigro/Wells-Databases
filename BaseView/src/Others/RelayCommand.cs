using System;
using System.Windows.Input;

namespace Wells.BaseView
{
    public class RelayCommand : ICustomCommand
    {

        readonly Action<object> _Execute;
        readonly Func<object, bool> _CanExecute;
        readonly Action<Exception> _OnError;
        readonly Action _Finally;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }


        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute, Action<Exception> onError)
        {
            _Execute = execute;
            _CanExecute = canExecute;
            _OnError = onError;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute, Action<Exception> onError, Action onFinish)
        {
            _Execute = execute;
            _CanExecute = canExecute;
            _OnError = onError;
            _Finally = onFinish;
        }

        public RelayCommand(Action<object> execute)
        {
            _Execute = execute;
            _CanExecute = new Func<object, bool>((param) => true);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _Execute(parameter);
                }
                catch (Exception ex)
                {
                    if (_OnError == null)
                    {
                        throw;
                    }
                    else
                    {
                        _OnError(ex);
                    }
                }
                finally
                {
                    _Finally?.Invoke();
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return _CanExecute != null ? _CanExecute(parameter) : true;
        }

        private void OnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            CanExecute(eventArgs);
        }

        public void OnCanExecuteChanged()
        {
            CanExecute(null);
        }
      
    }

    public interface ICustomCommand : ICommand
    {
        void OnCanExecuteChanged();
    }
}
