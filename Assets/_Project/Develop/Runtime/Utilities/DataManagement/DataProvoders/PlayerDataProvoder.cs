using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProvoders
{
    public class PlayerDataProvoder : DataProvoder<PlayerData>
    {
        public PlayerDataProvoder(ISaveLoadService saveLoadService) : base(saveLoadService)
        {
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

            walletData.Add(CurrencyTypes.Gold, 100);
            walletData.Add(CurrencyTypes.Diamond, 20);

            return walletData;
        }
    }
}
