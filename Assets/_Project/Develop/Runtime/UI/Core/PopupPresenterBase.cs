using System;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public abstract class PopupPresenterBase : IPresenter
    {
        public event Action<PopupPresenterBase> CloseRequest;

        protected abstract PopupViewBase PopupView { get; }

        public virtual void Initialize()
        {
            
        }

        public virtual void Dispose()
        {
            PopupView.CloseRequest -= OnCloseRequest;
        }

        public void Show()
        {
            OnPreShow();

            PopupView.Show();

            OnPostShow();
        }

        public void Hide(Action callback = null)
        {
            OnPreHide();

            PopupView.Hide();

            OnPostHide();

            callback?.Invoke();
        }

        protected virtual void OnPreShow() {
            PopupView.CloseRequest += OnCloseRequest;
        }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreHide() => PopupView.CloseRequest -= OnCloseRequest;
        protected virtual void OnPostHide() { }

        protected void OnCloseRequest() => CloseRequest?.Invoke(this);
    }
}
