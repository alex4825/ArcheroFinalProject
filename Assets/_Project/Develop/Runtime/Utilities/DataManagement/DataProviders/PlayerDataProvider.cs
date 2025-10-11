using Assets._Project.Develop.Runtime.Configs.Meta;
using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders
{
    public class PlayerDataProvider : DataProvider<PlayerData>
    {
        private ConfigsProviderService _configsProviderService;

        public PlayerDataProvider(ISaveLoadService saveLoadService, ConfigsProviderService configsProviderService) : base(saveLoadService)
        {
            _configsProviderService = configsProviderService;
        }

        protected override PlayerData GetOriginData()
        {
            return new PlayerData()
            {
                WalletData = InitWalletData(),
                StatsKoefs = InitStatsKoefs(),
                StatsCosts = InitStatsCosts(),
                CompletedLevels = new()
            };
        }

        private Dictionary<StatTypes, int> InitStatsCosts()
        {
            StatsConfig statsConfig = _configsProviderService.GetConfig<StatsConfig>();

            Dictionary<StatTypes, int> statsCosts = new();

            foreach (UpgradeConfig upgradeConfig in statsConfig.Configs)
            {
                statsCosts.Add(upgradeConfig.Type, upgradeConfig.Cost);
            }

            return statsCosts;
        }

        private Dictionary<StatTypes, float> InitStatsKoefs()
        {
            StatsConfig statsConfig = _configsProviderService.GetConfig<StatsConfig>();

            Dictionary<StatTypes, float> statsKoefs = new();

            foreach (UpgradeConfig upgradeConfig in statsConfig.Configs)
            {
                statsKoefs.Add(upgradeConfig.Type, upgradeConfig.Koef);
            }

            return statsKoefs;
        }

        private Dictionary<CurrencyTypes, int> InitWalletData()
        {
            Dictionary<CurrencyTypes, int> walletData = new();

            StartWalletConfig startWalletConfig = _configsProviderService.GetConfig<StartWalletConfig>();

            foreach (CurrencyTypes type in Enum.GetValues(typeof(CurrencyTypes)))
                walletData[type] = startWalletConfig.GetValueFor(type);

            return walletData;
        }
    }
}
