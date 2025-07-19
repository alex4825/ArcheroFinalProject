using System;
using System.Collections;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProvoders
{
    public abstract class DataProvoder<TData> where TData : ISaveData
    {
        private readonly ISaveLoadService _saveLoadService;

        private TData _data;

        protected DataProvoder(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public IEnumerator Load()
        {
            yield return _saveLoadService.Load<TData>(loadedData => _data = loadedData);
        }

        public IEnumerator Save()
        {
            yield return _saveLoadService.Save(_data);
        }

        public IEnumerator Exists(Action<bool> onExistsResult)
        {
            yield return _saveLoadService.Exists<TData>(result => onExistsResult?.Invoke(result));
        }

        public void Reset()
        {
            _data = GetOriginData();
        }

        protected abstract TData GetOriginData();
    }
}
