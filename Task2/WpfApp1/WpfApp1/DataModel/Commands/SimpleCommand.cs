using System;
using System.Windows.Input;

namespace WpfApp1.DataModel.Commands
{
    public class SimpleCommand : ICommand
    {

        public SimpleCommand()
        {
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
