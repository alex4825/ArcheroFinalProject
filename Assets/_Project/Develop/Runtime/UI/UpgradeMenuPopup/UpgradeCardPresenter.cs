using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using Assets._Project.Develop.Runtime.UI.Core;
using System;

namespace Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup
{
    public class UpgradeCardPresenter : IPresenter
    {
        public event Action<StatTypes> Selected;

        private readonly ViewsFactory _viewsFactory;
        private UpgradeCardView _upgradeCardView;
        private UpgradeConfig _upgradeConfig;
        private readonly StatsService _statsService;

        public UpgradeCardPresenter(
            UpgradeCardView upgradeCardView,
            UpgradeConfig upgradeConfig,
            ViewsFactory viewsFactory,
            StatsService statsService)
        {
            _upgradeCardView = upgradeCardView;
            _upgradeConfig = upgradeConfig;
            _viewsFactory = viewsFactory;
            _statsService = statsService;
        }

        public void Initialize()
        {
            _upgradeCardView.SetCost(_statsService.GetCostBy(_upgradeConfig.Type).ToString());
            _upgradeCardView.SetDescription(_upgradeConfig.Description.ToString());
            _upgradeCardView.SetLabel(_upgradeConfig.Label.ToString());
            _upgradeCardView.SetProfit((_upgradeConfig.Koef * 100).ToString("00") + "%");
            _upgradeCardView.SetIcon(_upgradeConfig.Icon);

            _upgradeCardView.Clicked += OnCardClicked;
        }

        public void Dispose()
        {
            _viewsFactory.Release(_upgradeCardView);
            _upgradeCardView.Clicked -= OnCardClicked;
        }

        private void OnCardClicked()
        {
            Selected?.Invoke(_upgradeConfig.Type);
        }
    }
}