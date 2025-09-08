using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagement
{
    public class PlayerData : ISaveData
    {
        public Dictionary<CurrencyTypes, int> WalletData;

        public List<int> CompletedLevels;

        public int VictoryCount;

        public int DefeatCount;
    }
}
