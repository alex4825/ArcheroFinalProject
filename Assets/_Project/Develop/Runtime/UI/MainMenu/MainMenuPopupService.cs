using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using UnityEngine;
using Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup;
using System;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPopupService : PopupService
    {
        private readonly MainMenuUIRoot _uiRoot;
        private readonly MainMenuPresentersFactory _mainMenuPresentersFactory;

        public MainMenuPopupService(
            ViewsFactory viewsFactory,
            ProjectPresentersFactory presentersFactory,
            MainMenuUIRoot uiRoot,
            MainMenuPresentersFactory mainMenuPresentersFactory)
            : base(viewsFactory, presentersFactory)
        {
            _uiRoot = uiRoot;
            _mainMenuPresentersFactory = mainMenuPresentersFactory;
        }

        protected override Transform PopupLayer => _uiRoot.PopupsLayer;

        public UpgradePopupPresenter OpenUpgradePopup(Action closedCallback = null)
        {
            UpgradePopupView view = ViewsFactory.Create<UpgradePopupView>(ViewIDs.UpgradePopupView, PopupLayer);

            UpgradePopupPresenter popup = _mainMenuPresentersFactory.CreateUpgradePopupPresenter(view);

            OnPopupCreated(popup, view, closedCallback);

            return popup;
        }
    }
}
