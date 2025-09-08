using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Environment
{
    public class LevelEnvironment : MonoBehaviour
    {
        [field:SerializeField] public Collider FortressCollider { get; private set; }
    }
}