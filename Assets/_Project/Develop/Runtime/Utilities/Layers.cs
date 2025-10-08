using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities
{
    public class Layers
    {
        public static readonly int Entity = LayerMask.NameToLayer("Entity");
        public static readonly LayerMask EntityMask = 1 << Entity;

        public static readonly int Environment = LayerMask.NameToLayer("Environment");
        public static readonly LayerMask EnvironmentMask = 1 << Environment;

        public static readonly int UI = LayerMask.NameToLayer("UI");
        public static readonly LayerMask UIMask = 1 << UI;
    }
}