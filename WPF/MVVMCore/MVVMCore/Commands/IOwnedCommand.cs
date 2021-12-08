using System.Windows.Input;

namespace MVVMCore.Commands
{
	public interface IOwnedCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
