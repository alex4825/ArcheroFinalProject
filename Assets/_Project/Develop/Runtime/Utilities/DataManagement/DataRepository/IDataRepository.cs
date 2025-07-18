using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagement.DataRepository
{
    public interface IDataRepository
    {
        IEnumerator Read(string key, Action<string> onRead);
        IEnumerator Write(string key, string serializedData);
        IEnumerator Remove(string key);
        IEnumerator Exists(string key, Action<bool> onExistsResult);
    }
}
