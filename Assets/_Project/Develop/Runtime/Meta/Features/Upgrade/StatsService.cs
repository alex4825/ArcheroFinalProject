using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features.Upgrade
{
    public class StatsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly ConfigsProviderService _configsProviderService;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private Dictionary<StatTypes, ReactiveVariable<float>> _statsKoefs = new();

        public StatsService(
            PlayerDataProvider playerDataProvider,
            ConfigsProviderService configsProviderService,
            ICoroutinesPerformer coroutinesPerformer)
        {
            _configsProviderService = configsProviderService;
            _playerDataProvider = playerDataProvider;

            playerDataProvider.RegisterReader(this);
            playerDataProvider.RegisterWriter(this);
            _coroutinesPerformer = coroutinesPerformer;
        }

        public float GetKoefBy(StatTypes type)
        {
            float koef = _statsKoefs.First(result => result.Key == type).Value.Value;

            if (koef == GetKoefFromConfigBy(type))
                return 1;
            else
                return 1 + koef;
        }

        public void Upgrade(StatTypes type)
        {
            _statsKoefs[type].Value *= ((GetKoefFromConfigBy(type) + 1));

            Debug.Log($"Апгрейд. {type.ToString()} = {_statsKoefs[type].Value}");

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAcync());
        }

        public void ReadFrom(PlayerData data)
        {
            foreach (var statToKoef in data.StatsData)
            {
                if (_statsKoefs.ContainsKey(statToKoef.Key))
                    _statsKoefs[statToKoef.Key].Value = statToKoef.Value;
                else
                    _statsKoefs.Add(statToKoef.Key, new ReactiveVariable<float>(GetKoefFromConfigBy(statToKoef.Key)));
            }
        }

        public void WriteTo(PlayerData data)
        {
            foreach (var statToKoef in _statsKoefs)
            {
                if (data.StatsData.ContainsKey(statToKoef.Key))
                    data.StatsData[statToKoef.Key] = statToKoef.Value.Value;
                else
                    data.StatsData.Add(statToKoef.Key, statToKoef.Value.Value);
            }
        }

        private float GetKoefFromConfigBy(StatTypes type) => _configsProviderService.GetConfig<StatsConfig>().GetBy(type).Koef;
    }
}