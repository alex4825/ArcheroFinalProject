using Assets._Project.Develop.Runtime.Configs.Meta;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
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
                WalletData = InitWalletData()
            };
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
