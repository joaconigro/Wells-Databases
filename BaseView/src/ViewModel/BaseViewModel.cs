using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Wells.Base;
using Wells.BaseView.ViewInterfaces;

namespace Wells.BaseView.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected Dictionary<string, IEnumerable<IValidator>> validators = new Dictionary<string, IEnumerable<IValidator>>();
        protected Dictionary<string, List<ValidationResult>> failures = new Dictionary<string, List<ValidationResult>>();
        protected Dictionary<string, List<ValidationResult>> validationResults = new Dictionary<string, List<ValidationResult>>();
        protected Dictionary<string, IEnumerable<ICommand>> commands = new Dictionary<string, IEnumerable<ICommand>>();
        protected virtual IView View { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        protected BaseViewModel(IView view)
        {
            if (view != null)
            {
                SetView(view);
            }
        }

        public void SetView(IView view)
        {
            View = view;
            NotifyPropertyChanged(nameof(CloseModalViewCommand));
            NotifyPropertyChanged(nameof(CloseNonModalViewCommand));
            OnSetView(view);
        }

        protected virtual void OnSetView(IView view) { }

        public bool IsValid(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) { return true; }
            return ValidationResult(propertyName).All(r => r.IsValid);
        }

        public List<ValidationResult> ValidationResult(string propertyName)
        {
            if (propertyName != null || validationResults.ContainsKey(propertyName))
            { return validationResults[propertyName]; }
            return new List<ValidationResult>();
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            RaiseCommandUpdates(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ShowErrorMessage(string message)
        {
            if (View != null)            
            { 
                SharedBaseView. ShowErrorMessageBox(View, message); 
            }
        }

        protected void ShowMessage(string message, string title)
        {
            if (View != null)
            {
                SharedBaseView.ShowOkOnkyMessageBox(View, message, title);
            }
        }

        protected bool ShowYesNoMessageBox(string message, string title)
        {
            if (View != null) { return SharedBaseView.ShowYesNoMessageBox(View, message, title); }
            return false;
        }

        protected void OnError(Exception ex)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ExceptionHandler.Handle(ex, false);
                var errorMessage = ExceptionHandler.GetAllMessages(ex).Trim();
                ShowErrorMessage(errorMessage);
            });
        }

        protected void SetValue<T>(ref T value, T newValue, [CallerMemberName()] string propertyName = null, bool forceAssignment = false)
        {
            if ((newValue == null && value != null) || (newValue != null && (!newValue.Equals(value) || forceAssignment)))
            {
                OnPropertyChanging(propertyName);
                value = newValue;
                if (validators.ContainsKey(propertyName)) { Validate(propertyName, newValue); }
                NotifyPropertyChanged(propertyName);
                OnPropertyChanged(propertyName);
                UpdateCommandsThatDependsOnFailures();
            }
        }

        protected virtual void OnPropertyChanging(string propertyName) { }

        protected virtual void OnPropertyChanged(string propertyName) { }


        protected virtual void OnPropertyValidated(string propertyName) { }


        protected virtual void RaiseCommandUpdates(string propertyName)
        {
            if (commands.ContainsKey(propertyName))
            {
                foreach (ICustomCommand command in commands[propertyName])
                {
                    command.OnCanExecuteChanged();
                }
            }
        }

        protected abstract void SetValidators();

        protected abstract void SetCommandUpdates();

        protected void Add(string propertyName, IEnumerable<IValidator> validators)
        {
            if (this.validators.ContainsKey(propertyName))
            {
                var list = this.validators[propertyName].ToList();
                list.AddRange(validators);
                this.validators[propertyName] = list;
            }
            else
            { this.validators.Add(propertyName, validators); }
        }

        protected void Add(string propertyName, IValidator validator)
        {
            if (validators.ContainsKey(propertyName))
            {
                var list = validators[propertyName].ToList();
                list.Add(validator);
                this.validators[propertyName] = list;
            }
            else
            { this.validators.Add(propertyName, new List<IValidator>() { validator }); }
        }

        protected void Add(string propertyName, IEnumerable<ICommand> commands)
        {
            if (this.commands.ContainsKey(propertyName))
            {
                var list = this.commands[propertyName].ToList();
                list.AddRange(commands);
                this.commands[propertyName] = list;
            }
            else
            { this.commands.Add(propertyName, commands); }
        }

        protected void Add(string propertyName, ICommand command)
        {
            if (commands.ContainsKey(propertyName))
            {
                var list = commands[propertyName].ToList();
                list.Add(command);
                commands[propertyName] = list;
            }
            else
            { commands.Add(propertyName, new List<ICommand>() { command }); }
        }

        protected virtual void Initialize()
        {
            SetValidators();
            SetCommandUpdates();
        }

        protected void UpdateCommandsThatDependsOnFailures()
        {
            NotifyPropertyChanged(nameof(Errors));
            NotifyPropertyChanged(nameof(HasFailures));
            RaiseCommandUpdates(nameof(Errors));
            RaiseCommandUpdates(nameof(HasFailures));

        }

        protected void ValidateAll()
        {
            foreach (var key in validators.Keys)
            {
                Validate(key, Microsoft.VisualBasic.Interaction.CallByName(this, key, Microsoft.VisualBasic.CallType.Get));
            }
        }


        protected void Validate<T>(string propertyName, T value)
        {
            if (value == null)
            {
                var errors = new List<ValidationFailure>() { new ValidationFailure(propertyName, "Error: hay una propiedad nula") };
                validationResults[propertyName] = new List<ValidationResult> { new ValidationResult(errors) };
            }
            else
            {
                var results = new List<ValidationResult>();
                foreach (var v in validators[propertyName])
                {
                    results.Add(v.Validate(value));
                }

                validationResults[propertyName] = results;
            }
            if (IsValid(propertyName)) { OnPropertyValidated(propertyName); }
        }

        public string Errors
        {
            get
            {
                var results = new List<ValidationResult>();
                foreach (var r in validationResults.Values)
                {
                    results.AddRange(r);
                }

                return string.Join(Environment.NewLine, from v in results
                                                        where !v.IsValid
                                                        select string.Join(Environment.NewLine, v.Errors.Select(e => e.ErrorMessage)));
            }
        }

        public bool HasFailures
        {
            get
            {
                if (validators.Any() && validationResults.Count != validators.Count) { return true; }
                return validationResults.Values.Any(r => r.Any(vr => !vr.IsValid));
            }
        }

        public ICommand CloseModalViewCommand
        {
            get => new RelayCommand((param) =>
            {
                bool? result;
                if (param == null) { result = false; }
                else { result = (bool?)param; }
                View?.CloseView(result);
            }, (obj) => View != null, OnError);
        }


        public ICommand CloseNonModalViewCommand
        {
            get => new RelayCommand((param) =>
            {
                View?.CloseView();
            }, (obj) => View != null, OnError);
        }

    }
}
