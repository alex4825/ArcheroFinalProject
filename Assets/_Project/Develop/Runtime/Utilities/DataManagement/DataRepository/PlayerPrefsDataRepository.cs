using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.DataRepository
{
    public class PlayerPrefsDataRepository : IDataRepository
    {
        public IEnumerator Exists(string key, Action<bool> onExistsResult)
        {
            onExistsResult?.Invoke(PlayerPrefs.HasKey(key));

            yield break;
        }

        public IEnumerator Read(string key, Action<string> onRead)
        {
            onRead?.Invoke(PlayerPrefs.GetString(key));

            yield break;
        }

        public IEnumerator Remove(string key)
        {
            PlayerPrefs.DeleteKey(key);

            yield break;
        }

        public IEnumerator Write(string key, string serializedData)
        {
            PlayerPrefs.SetString(key, serializedData);

            yield break;
        }
    }
}
