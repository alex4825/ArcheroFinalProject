using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode
{
    public class ExplodeRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplodeDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ExplodedEvent : IEntityComponent
    {
        public ReactiveEvent<Vector3> Value;
    }

    public class MustExplode : IEntityComponent
    {
        public ICondition Value;
    }
}