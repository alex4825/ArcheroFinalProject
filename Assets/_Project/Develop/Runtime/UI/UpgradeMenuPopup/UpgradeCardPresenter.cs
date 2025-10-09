using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.UI.Core;

namespace Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup
{
    public class UpgradeCardPresenter : IPresenter
    {
        private readonly ViewsFactory _viewsFactory;
        private UpgradeCardView _upgradeCardView;
        private UpgradeConfig _upgradeConfig;

        public UpgradeCardPresenter(
            UpgradeCardView upgradeCardView,
            UpgradeConfig upgradeConfig,
            ViewsFactory viewsFactory)
        {
            _upgradeCardView = upgradeCardView;
            _upgradeConfig = upgradeConfig;
            _viewsFactory = viewsFactory;
        }

        public void Initialize()
        {
            _upgradeCardView.SetCost(_upgradeConfig.Cost.ToString());
            _upgradeCardView.SetDescription(_upgradeConfig.Description.ToString());
            _upgradeCardView.SetLabel(_upgradeConfig.Label.ToString());
            _upgradeCardView.SetProfit((_upgradeConfig.Koef).ToString());
            _upgradeCardView.SetIcon(_upgradeConfig.Icon);
        }

        public void Dispose()
        {
            _viewsFactory.Release(_upgradeCardView);
        }
    }
}