using Assets._Project.Develop.Runtime.UI.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class DefeatPopupView : PopupViewBase
    {
        public event Action ContinueClicked;
        public event Action RestartClicked;

        [SerializeField] private TMP_Text _title;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButton;

        public void SetTitle(string title) => _title.text = title;

        public void OnRestartButtonCliked() => RestartClicked?.Invoke();

        public void OnContinueButtonCliked() => ContinueClicked?.Invoke();

        protected override void OnPreShow()
        {
            base.OnPreShow();

            _continueButton.onClick.AddListener(OnContinueButtonCliked);
            _restartButton.onClick.AddListener(OnRestartButtonCliked);
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _continueButton.onClick.RemoveListener(OnContinueButtonCliked);
            _restartButton.onClick.RemoveListener(OnRestartButtonCliked);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnContinueButtonCliked);
            _restartButton.onClick.RemoveListener(OnRestartButtonCliked);
        }
    }
}