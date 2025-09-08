using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.DataManipulation
{
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        [field: SerializeField]
        public TKey Key { get; private set; }

        [field: SerializeField]
        public TValue Value { get; private set; }
    }
}