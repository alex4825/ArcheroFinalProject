using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagement
{
    public interface IConfigsLoader
    {
        IEnumerator LoadAcync(Action<Dictionary<Type, object>> onConfigsLoaded);
    }
}