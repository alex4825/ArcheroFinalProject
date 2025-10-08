using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI
{
    [RequireComponent(typeof(Image))]
    public class PreserveAspectSetter : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Image>().preserveAspect = true;
        }
    }
}