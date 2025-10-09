using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.MainMenu;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup
{
    public class UpgradePopupPresenter : PopupPresenterBase
    {
        private UpgradePopupView _upgradePopupView;
        private readonly ViewsFactory _viewsFactory;
        private readonly MainMenuPresentersFactory _presentersFactory;

        private List<IPresenter> _cardsPresenters = new();

        public UpgradePopupPresenter(
            UpgradePopupView upgradePopupView,
            ICoroutinesPerformer coroutinesPerformer,
            ViewsFactory viewsFactory,
            MainMenuPresentersFactory presentersFactory) : base(coroutinesPerformer)
        {
            _upgradePopupView = upgradePopupView;
            _viewsFactory = viewsFactory;
            _presentersFactory = presentersFactory;
        }

        protected override PopupViewBase PopupView => _upgradePopupView;

        public override void Initialize()
        {
            base.Initialize();

            foreach (UpgradeConfig config in _upgradePopupView.UpgradesConfig.Configs)
            {
                UpgradeCardView upgradeCardView = _viewsFactory.Create<UpgradeCardView>(ViewIDs.UpgradeCardView, _upgradePopupView.CardsContainer);

                UpgradeCardPresenter upgradeCardPresenter = _presentersFactory.CreateUpgradeCardPresenter(
                    upgradeCardView, 
                    config);

                upgradeCardPresenter.Initialize();
                _cardsPresenters.Add(upgradeCardPresenter);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (IPresenter presenter in _cardsPresenters)
            {
                presenter.Dispose();
            }
        }
    }
}