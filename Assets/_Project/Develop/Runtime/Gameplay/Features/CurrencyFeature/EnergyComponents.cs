using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

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

    public class FullEnergyEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class AddEnergyCountRequest : IEntityComponent
    {
        public ReactiveEvent<float> Value;
    }

    public class SubtractEnergyCountRequest : IEntityComponent
    {
        public ReactiveEvent<float> Value;
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