using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public class PopupAnimationsCreator
    {
        public static Sequence CreateShowAnimation(
            CanvasGroup body,
            Image anticlicker,
            PopupAnimationTypes popupAnimationType,
            float anticlickerMaxAlpha)
        {
            switch (popupAnimationType)
            {
                case PopupAnimationTypes.None:
                    return DOTween.Sequence();

                case PopupAnimationTypes.Expand:
                    return DOTween.Sequence()
                        .Append(anticlicker
                            .DOFade(anticlickerMaxAlpha, 0.2f)
                            .From(0))
                        .Join(body.transform
                            .DOScale(1, 0.5f)
                            .From(0).SetEase(Ease.OutBack));

                case PopupAnimationTypes.Fade:
                    return DOTween.Sequence()
                        .Append(anticlicker
                            .DOFade(anticlickerMaxAlpha, 0.2f)
                            .From(0))
                        .Join(body
                            .DOFade(1, 0.3f)
                            .From(0));

                default:
                    throw new ArgumentException(nameof(popupAnimationType));
            }
        }

        public static Sequence CreateHideAnimation(
            CanvasGroup body,
            Image anticlicker,
            PopupAnimationTypes popupAnimationType,
            float anticlickerMaxAlpha)
        {
            switch (popupAnimationType)
            {
                case PopupAnimationTypes.None:
                    return DOTween.Sequence();

                case PopupAnimationTypes.Expand:
                    return DOTween.Sequence()
                        .Append(anticlicker
                            .DOFade(0, 0.2f)
                            .From(anticlicker.color.a))
                        .Join(body.transform
                            .DOScale(0, 0.5f)
                            .From(1).SetEase(Ease.InSine));

                case PopupAnimationTypes.Fade:
                    return DOTween.Sequence()
                        .Append(anticlicker
                            .DOFade(0, 0.2f)
                            .From(anticlicker.color.a))
                        .Join(body
                            .DOFade(0, 0.3f)
                            .From(body.alpha));

                default:
                    throw new ArgumentException(nameof(popupAnimationType));
            }
        }
    }
}
