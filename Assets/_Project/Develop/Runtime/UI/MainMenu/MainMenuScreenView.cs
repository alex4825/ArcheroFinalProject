using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action OpenTestPopupButtonClicked;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }

        [SerializeField] private Button _openTestPopupButton;

        private void OnEnable()
        {
            _openTestPopupButton.onClick.AddListener(OnOpenTestPopupButtonClicked);
        }

        private void OnDisable()
        {

            _openTestPopupButton.onClick.RemoveListener(OnOpenTestPopupButtonClicked);
        }

        private void OnOpenTestPopupButtonClicked() => OpenTestPopupButtonClicked?.Invoke();
    }
}
