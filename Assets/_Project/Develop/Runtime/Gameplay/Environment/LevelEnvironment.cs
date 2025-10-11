using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Environment
{
    public class LevelEnvironment : MonoBehaviour
    {
        [field: SerializeField] public MonoEntity Fortress { get; private set; }
        [field: SerializeField] public Collider FortressCollider { get; private set; }
    }
}