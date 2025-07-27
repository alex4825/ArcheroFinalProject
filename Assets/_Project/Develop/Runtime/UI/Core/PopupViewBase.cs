using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public abstract class PopupViewBase : MonoBehaviour, IShovableView
    {
        public event Action CloseRequest;

        [SerializeField] private CanvasGroup _mainGroup;

        private void Awake()
        {
            _mainGroup.alpha = 0;
        }

        public void OnCloseButtonClicked() => CloseRequest?.Invoke();

        public void Show()
        {
            OnPreShow();

            //Анимация появления
            _mainGroup.alpha = 1;

            OnPostShow();
        }

        public void Hide()
        {
            OnPreHide();

            //Анимация исчезновения
            _mainGroup.alpha = 0;

            OnPostHide();
        }

        protected virtual void OnPreShow() { }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreHide() { }

        protected virtual void OnPostHide() { }

    }
}
