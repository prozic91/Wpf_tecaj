using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf_Tecaj.ViewModel.Command
{
    public class SimpleCommand : ICommand
    {
        public MainWindowViewModel ViewModel { get; set; }

        public SimpleCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.ViewModel.SimpleMethod();
        }
    }
}
