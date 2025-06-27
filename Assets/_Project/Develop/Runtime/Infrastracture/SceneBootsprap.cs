using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture
{
    public abstract class SceneBootsprap : MonoBehaviour
    {
        public abstract IEnumerator Initialize(DIContainer container, IInputSceneArgs sceneArgs = null);

        public abstract void Run(); 
    }
}
