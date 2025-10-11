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
        private const int CostMultiplier = 2;

        private readonly ConfigsProviderService _configsProviderService;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private Dictionary<StatTypes, ReactiveVariable<float>> _statsKoefs = new();
        private Dictionary<StatTypes, ReactiveVariable<int>> _statsCosts = new();

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
            float koef = _statsKoefs[type].Value;

            float configKoef = GetKoefFromConfigBy(type);

            if (koef == configKoef)
                return 1;
            else
                return 1 + koef;
        }

        public int GetCostBy(StatTypes type)
        {
            return _statsCosts[type].Value;
        }

        public void Upgrade(StatTypes type)
        {
            _statsKoefs[type].Value *= ((GetKoefFromConfigBy(type) + 1));
            _statsCosts[type].Value *= CostMultiplier;

            Debug.Log($"Апгрейд. {type.ToString()}: коэффициент - {_statsKoefs[type].Value}, цена -  {_statsCosts[type].Value}");

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAcync());
        }

        public void ReadFrom(PlayerData data)
        {
            foreach (var statToKoef in data.StatsKoefs)
            {
                if (_statsKoefs.ContainsKey(statToKoef.Key))
                    _statsKoefs[statToKoef.Key].Value = statToKoef.Value;
                else
                    _statsKoefs.Add(statToKoef.Key, new ReactiveVariable<float>(statToKoef.Value));
            }

            foreach (var statToCost in data.StatsCosts)
            {
                if (_statsCosts.ContainsKey(statToCost.Key))
                    _statsCosts[statToCost.Key].Value = statToCost.Value;
                else
                    _statsCosts.Add(statToCost.Key, new ReactiveVariable<int>(statToCost.Value));
            }
        }

        public void WriteTo(PlayerData data)
        {
            foreach (var statToKoef in _statsKoefs)
            {
                if (data.StatsKoefs.ContainsKey(statToKoef.Key))
                    data.StatsKoefs[statToKoef.Key] = statToKoef.Value.Value;
                else
                    data.StatsKoefs.Add(statToKoef.Key, statToKoef.Value.Value);
            }

            foreach (var statToCost in _statsCosts)
            {
                if (data.StatsCosts.ContainsKey(statToCost.Key))
                    data.StatsCosts[statToCost.Key] = statToCost.Value.Value;
                else
                    data.StatsCosts.Add(statToCost.Key, statToCost.Value.Value);
            }
        }

        private float GetKoefFromConfigBy(StatTypes type) => _configsProviderService.GetConfig<StatsConfig>().GetBy(type).Koef;
    }
}