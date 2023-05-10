using UnityEngine;

namespace FroguesFramework
{
    public class MaterialInstanceContainer : MonoBehaviour, IAbleToDisablePreVisualization
    {
        private Material _materialInstance;
        public Material MaterialInstance => _materialInstance;

        private void Start()
        {
            _materialInstance = GetComponent<Renderer>().material;
            AddMySelfToEntryPoint();
        }

        public void EnableOutline(bool value)
        {
            _materialInstance.SetInt("_OutlineEnabled", value ? 1 : 0);
        }

        public void DisablePreVisualization()
        {
            EnableOutline(false);
        }

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);
    }
}