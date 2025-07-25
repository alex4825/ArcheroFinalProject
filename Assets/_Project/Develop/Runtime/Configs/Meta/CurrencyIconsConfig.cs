using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Meta
{
    [CreateAssetMenu(fileName = "CurrencyIconsConfig", menuName = "Configs/Meta/Wallet/CurrencyIconsConfig")]
    public class CurrencyIconsConfig : ScriptableObject
    {
        [SerializeField] private List<CurrencyConfig> _configs;

        public Sprite GetSpriteFor(CurrencyTypes currencyType)
            => _configs.First(config => config.Type == currencyType).Sprite;

        [Serializable]
        private class CurrencyConfig
        {
            [field: SerializeField] public CurrencyTypes Type { get; private set; }
            [field: SerializeField] public Sprite Sprite { get; private set; }
        }
    }
}
