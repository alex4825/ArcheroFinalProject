using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons
{
    public class DefenderIconView : IconTextView, IView
    {
        public event Action<DefenderIconView> Clicked;

        [SerializeField] private Image _background;

        public DefenderConfig DefenderConfig { get; private set; }

        public void SetBackgroundColor(Color color) => _background.color = color;

        public void SetConfig(DefenderConfig config) => DefenderConfig = config;

        public void OnClick() => Clicked?.Invoke(this);
    }
}
