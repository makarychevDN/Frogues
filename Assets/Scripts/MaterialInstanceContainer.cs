using UnityEngine;

namespace FroguesFramework
{
    public class MaterialInstanceContainer : MonoBehaviour
    {
        private Material _materialInstance;

        private void Start()
        {
            _materialInstance = GetComponent<Renderer>().material;
        }
    }
}