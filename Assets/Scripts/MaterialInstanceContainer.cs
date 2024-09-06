using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace FroguesFramework
{
    public class MaterialInstanceContainer : MonoBehaviour, IAbleToDisablePreVisualization
    {
        private Unit _unit;
        private Material _materialInstance;
        public Material MaterialInstance => _materialInstance;

        public void Init(Unit unit)
        {
            _unit = unit;
            _materialInstance = GetComponent<Renderer>().material;
            AddSelfToTheList();
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
            RemoveSelfFromTheList();
        }

        public void AddSelfToTheList() => _unit.AddAbleToDisablePrevisualizationObject(this);

        public void RemoveSelfFromTheList() => _unit.RemoveAbleToDisablePrevisualizationObject(this);
    }
}