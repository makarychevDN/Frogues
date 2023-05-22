using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitDescriptionPanel : MonoBehaviour, IAbleToDisablePreVisualization
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text textField;

        private void Start()
        {
            AddMySelfToEntryPoint();
        }

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public void Activate(string text)
        {
            panel.SetActive(true);
            textField.text = text;
        }

        public void DisablePreVisualization()
        {
            panel.SetActive(false);
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);
    }
}