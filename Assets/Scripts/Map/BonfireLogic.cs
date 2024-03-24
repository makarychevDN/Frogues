using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class BonfireLogic : MonoBehaviour
    {
        [SerializeField] private Vector2Int restCelCoordinates;
        [SerializeField] private List<GameObject> visualizationGameObjects;
        private Cell _restCell;

        private void Start()
        {
            _restCell = EntryPoint.Instance.Map.GetCell(restCelCoordinates);
            _restCell.OnBecameFull.AddListener(EnableBonfire);
        }

        private void EnableBonfire()
        {
            EntryPoint.Instance.EnableBonfireRestPanel(true);
            _restCell.OnBecameFull.RemoveListener(EnableBonfire);
            visualizationGameObjects.ForEach(go => go.SetActive(false));
        }
    }
}