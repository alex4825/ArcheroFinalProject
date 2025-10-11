using Assets._Project.Develop.Runtime.UI.CommonViews;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.Wave
{
    public class RestTimerView : SimpleView
    {
        [SerializeField] private Image _fill;
        
        [field: SerializeField] public Gradient ChangeGradient;

        public void SetFill(float koef) => _fill.fillAmount = koef;

        public void SetColor(float koef) => _fill.color = ChangeGradient.Evaluate(koef);
    }
}