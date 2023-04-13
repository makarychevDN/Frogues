using UnityEngine;

namespace FroguesFramework
{
    public class MaterialInstanceContainer : MonoBehaviour, IAbleToDisablePreVisualization
    {
        private Material _materialInstance;
        public Material MaterialInstance => _materialInstance;

        private void Awake()
        {
            _materialInstance = GetComponent<Renderer>().material;
        }

        public void EnableOutline(bool value)
        {
            _materialInstance.SetInt("_OutlineEnabled", value ? 1 : 0);
        }

        public void DisablePreVisualization()
        {
            EnableOutline(false);
        }
    }
}