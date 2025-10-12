using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.MainMenu;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup
{
    public class UpgradePopupPresenter : PopupPresenterBase
    {
        private UpgradePopupView _upgradePopupView;
        private StatsConfig _upgradesConfig;
        private readonly ViewsFactory _viewsFactory;
        private readonly MainMenuPresentersFactory _presentersFactory;
        private readonly StatsService _upgradeService;
        private readonly WalletService _wallet;
        private readonly SoundLauncher _soundLauncher;

        private List<UpgradeCardPresenter> _cardsPresenters = new();

        public UpgradePopupPresenter(
            UpgradePopupView upgradePopupView,
            StatsConfig statsConfig,
            StatsService upgradeService,
            ICoroutinesPerformer coroutinesPerformer,
            ViewsFactory viewsFactory,
            MainMenuPresentersFactory presentersFactory,
            WalletService wallet,
            SoundLauncher soundLauncher) : base(coroutinesPerformer)
        {
            _upgradePopupView = upgradePopupView;
            _upgradesConfig = statsConfig;
            _viewsFactory = viewsFactory;
            _presentersFactory = presentersFactory;
            _upgradeService = upgradeService;
            _wallet = wallet;
            _soundLauncher = soundLauncher;
        }

        protected override PopupViewBase PopupView => _upgradePopupView;

        public override void Initialize()
        {
            base.Initialize();

            foreach (UpgradeConfig config in _upgradesConfig.Configs)
            {
                UpgradeCardView upgradeCardView = _viewsFactory.Create<UpgradeCardView>(ViewIDs.UpgradeCardView, _upgradePopupView.CardsContainer);

                UpgradeCardPresenter upgradeCardPresenter = _presentersFactory.CreateUpgradeCardPresenter(
                    upgradeCardView,
                    config);

                upgradeCardPresenter.Selected += OnCardSelected;
                upgradeCardPresenter.Initialize();
                _cardsPresenters.Add(upgradeCardPresenter);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (UpgradeCardPresenter presenter in _cardsPresenters)
            {
                presenter.Dispose();
                presenter.Selected -= OnCardSelected;
            }
        }

        private void OnCardSelected(StatTypes type)
        {
            if (_wallet.Enough(CurrencyTypes.Diamond, _upgradesConfig.GetBy(type).Cost))
            {
                _wallet.Spend(CurrencyTypes.Diamond, _upgradesConfig.GetBy(type).Cost);
                _upgradeService.Upgrade(type);

                _soundLauncher.PlayUpgradeApplySound();
            }
            else
            {
                _soundLauncher.PlayUpgradeCancelSound();
            }

            OnCloseRequest();
        }
    }
}