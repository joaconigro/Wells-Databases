using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wells.BaseView
{
    public class AsyncCommand : IAsyncCommand
    {
        bool _IsExecuting;
        readonly Func<Task> _Execute;
        readonly Func<bool> _CanExecute;
        readonly Action<Exception> _OnError;
        readonly Action _OnFinish;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute, Action<Exception> onError)
        {
            _Execute = execute;
            _CanExecute = canExecute;
            _OnError = onError;
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute, Action<Exception> onError, Action onFinish)
        {
            _Execute = execute;
            _CanExecute = canExecute;
            _OnError = onError;
            _OnFinish = onFinish;
        }

        public AsyncCommand(Func<Task> execute)
        {
            _Execute = execute;
            _CanExecute = new Func<bool>(() => true);
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync().ConfigureAwait(false);
        }

        public bool CanExecute()
        {
            return !_IsExecuting && (_CanExecute != null ? _CanExecute.Invoke() : true);
        }

        public bool CanExecute(object parameter)
        {
            return _CanExecute != null ? _CanExecute.Invoke() : true;
        }


        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _IsExecuting = true;
                    await Task.Run(() => _Execute.Invoke()).ConfigureAwait(false);
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
                    _OnFinish?.Invoke();
                    _IsExecuting = false;
                }
            }
            OnCanExecuteChanged();
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

    public interface IAsyncCommand : ICustomCommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}
