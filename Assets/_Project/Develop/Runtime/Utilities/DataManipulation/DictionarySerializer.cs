using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.DataManipulation
{
    public class DictionarySerializer
    {
        public static Dictionary<TKey, TValue> GetFrom<TKey, TValue>(List<SerializableKeyValuePair<TKey, TValue>> list)
        {
            Dictionary<TKey, TValue> resultDictionary = new();

            foreach (SerializableKeyValuePair<TKey, TValue> item in list)
            {
                if (resultDictionary.ContainsKey(item.Key))
                    throw new InvalidCastException($"Key {item.Key} must not be repeated");

                resultDictionary.Add(item.Key, item.Value);
            }

            return resultDictionary;
        }
    }
}