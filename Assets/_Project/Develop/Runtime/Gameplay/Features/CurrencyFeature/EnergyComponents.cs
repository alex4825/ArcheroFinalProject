using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.CurrencyFeature
{
    public class InitialEnergyCount : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CurrentEnergyCount : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class RecoveryEnergyCountKoef : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TimeToRecoverEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}