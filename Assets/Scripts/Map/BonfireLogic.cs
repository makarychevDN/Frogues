using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class BonfireLogic : MonoBehaviour
    {
        [SerializeField] private Cell restCell;
        [SerializeField] private Cell exitCell;
        [SerializeField] private List<GameObject> visualizationGameObjects;

        private void Start()
        {
            restCell.OnBecameFull.AddListener(EnableBonfire);
            exitCell.OnBecameFull.AddListener(EntryPoint.Instance.StartNextRoom);
        }

        private void EnableBonfire()
        {
            EntryPoint.Instance.EnableBonfireRestPanel(true);
            restCell.OnBecameFull.RemoveListener(EnableBonfire);
        }
    }
}