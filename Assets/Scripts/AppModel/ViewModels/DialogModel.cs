using System.Windows.Input;
using StlVault.Util;
using StlVault.Util.Commands;
using StlVault.Util.Messaging;

namespace StlVault.AppModel.ViewModels
{
    internal abstract class DialogModel<TShowMessage> : ModelBase, IDialogViewModel, IMessageReceiver<TShowMessage>
    {
        private bool _shown;

        public bool Shown
        {
            get => _shown;
            private set => SetValueAndNotify(ref _shown, value);
        }

        public ICommand AcceptCommand { get; }
        public ICommand CancelCommand { get; }

        public DialogModel()
        {
            AcceptCommand = new DelegateCommand(CanAccept, Accept);
            CancelCommand = new DelegateCommand(CanCancel, Cancel);
        }

        protected void CanAcceptChanged() => AcceptCommand.OnCanExecuteChanged();
        protected virtual bool CanAccept() => true;
        protected abstract void OnAccept();
        private void Accept()
        {
            OnAccept();
            Shown = false;
            Reset();
        }

        protected void CanCancelChanged() => CancelCommand.OnCanExecuteChanged();
        protected virtual bool CanCancel() => true;
        protected virtual void OnCancel() {}
        private void Cancel()
        {
            OnCancel();
            Shown = false;
            Reset();
        }

        public void Receive(TShowMessage message) => Show(message);
        protected virtual void OnShown(TShowMessage message) {}
        private void Show(TShowMessage message)
        {
            OnShown(message);
            Shown = true;
            Reset();
        }

        protected abstract void Reset();
    }
}