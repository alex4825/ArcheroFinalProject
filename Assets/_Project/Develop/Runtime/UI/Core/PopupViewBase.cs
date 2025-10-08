using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public abstract class PopupViewBase : MonoBehaviour, IShowableView
    {
        public event Action CloseRequest;

        [SerializeField] private CanvasGroup _mainGroup;
        [SerializeField] private Image _anticklicker;
        [SerializeField] private CanvasGroup _body;

        [SerializeField] private PopupAnimationTypes _animationType;

        private float _anticklickerDefaultAlpha;

        private Tween _currentAnimation;

        private void Awake()
        {
            _anticklickerDefaultAlpha = _anticklicker.color.a;
            _mainGroup.alpha = 0;
        }

        public void OnCloseButtonClicked() => CloseRequest?.Invoke();

        public Tween Show()
        {
            KillCurrentAnimation();

            OnPreShow();

            //Анимация появления
            _mainGroup.alpha = 1;

            Sequence animation = PopupAnimationsCreator.CreateShowAnimation(_body, _anticklicker, _animationType, _anticklickerDefaultAlpha);

            ModifyShowAnimation(animation);

            animation.OnComplete(OnPostShow);

            return _currentAnimation = animation.SetUpdate(true).Play();
        }

        public Tween Hide()
        {
            KillCurrentAnimation();

            OnPreHide();

            //Анимация исчезновения
            Sequence animation = PopupAnimationsCreator.CreateHideAnimation(_body, _anticklicker, _animationType, _anticklickerDefaultAlpha);

            ModifyHideAnimation(animation);

            animation.OnComplete(OnPostHide);

            return _currentAnimation = animation.SetUpdate(true).Play();
        }

        protected virtual void ModifyShowAnimation(Sequence animation) { }

        protected virtual void ModifyHideAnimation(Sequence animation) { }

        protected virtual void OnPreShow() { }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreHide() { }

        protected virtual void OnPostHide() { }

        private void KillCurrentAnimation()
        {
            if (_currentAnimation != null)
                _currentAnimation.Kill();
        }

        private void OnDestroy()
        {

            KillCurrentAnimation();
        }
    }
}
