using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons
{
    public class DefenderIconView : IconTextView, IView
    {
        public event Action<DefenderConfig> Clicked;

        private DefenderConfig _defenderConfig;

        public void SetConfig(DefenderConfig config) => _defenderConfig = config;

        public void OnClick() => Clicked?.Invoke(_defenderConfig);
    }
}
