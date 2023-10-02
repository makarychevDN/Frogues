using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class BonfireLogic : MonoBehaviour
    {
        [SerializeField] private Cell getHealingCell;
        [SerializeField] private Cell getScoreCell;
        [SerializeField] private Cell exitCell;
        [SerializeField] private int scoreValue;
        [SerializeField] private List<GameObject> visualizationGameObjects;

        private void Start()
        {
            getHealingCell.OnBecameFullByUnit.AddListener(HealUnit);
            getScoreCell.OnBecameFull.AddListener(IncreaseScore);
            exitCell.OnBecameFull.AddListener(EntryPoint.Instance.StartNextRoom);
        }

        private void CancelEffects()
        {
            visualizationGameObjects.ForEach(go => go.SetActive(false));
            getHealingCell.OnBecameFullByUnit.RemoveListener(HealUnit);
            getScoreCell.OnBecameFull.RemoveListener(IncreaseScore);
        }

        private void HealUnit(Unit unit)
        {
            unit.Health.TakeHealing(EntryPoint.Instance.BonfireHealingValue);
            CancelEffects();
        }

        private void IncreaseScore()
        {
            EntryPoint.Instance.IncreaseScore(scoreValue);
            CancelEffects();
        }
    }
}