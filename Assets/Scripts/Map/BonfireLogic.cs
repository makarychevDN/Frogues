using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class BonfireLogic : MonoBehaviour
    {
        [SerializeField] private Vector2Int restCelCoordinates;
        [SerializeField] private List<GameObject> visualizationGameObjects;
        [SerializeField] private GameObject bonfireIcon;
        private Cell _restCell;

        private void Start()
        {
            if (EntryPoint.Instance.Score == 0)
                return;

            _restCell = EntryPoint.Instance.Map.GetCell(restCelCoordinates);
            _restCell.OnBecameFullByUnit.AddListener(EnableBonfire);
            bonfireIcon.SetActive(true);
        }

        private void EnableBonfire(Unit unit)
        {
            EntryPoint.Instance.EnableBonfireRestPanel(true);
            unit.MovementAbility.ResetPath();
            _restCell.OnBecameFullByUnit.RemoveListener(EnableBonfire);
            visualizationGameObjects.ForEach(go => go.SetActive(false));
        }
    }
}