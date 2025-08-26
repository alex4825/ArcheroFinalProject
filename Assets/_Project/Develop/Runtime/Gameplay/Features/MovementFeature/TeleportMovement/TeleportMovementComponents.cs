using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature.TeleportMovement
{
    public class TeleportEnergyCost : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportMaxRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportedEvent : IEntityComponent
    {
        public ReactiveEvent<Vector3> Value;
    }

    public class CanTeleport : IEntityComponent
    {
        public ICompositeCondition Value;
    }
}