using UnityEngine;

namespace FroguesFramework
{
    public class MaterialInstanceContainer : MonoBehaviour
    {
        private Material _materialInstance;
        public Material MaterialInstance => _materialInstance;

        private void Start()
        {
            _materialInstance = GetComponent<Renderer>().material;
        }
    }
}