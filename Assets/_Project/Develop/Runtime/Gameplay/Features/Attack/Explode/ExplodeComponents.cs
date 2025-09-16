using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode
{
    public class ExplodeRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}