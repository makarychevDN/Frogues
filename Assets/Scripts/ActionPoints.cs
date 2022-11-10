using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class ActionPoints : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private int currentPoints;
        [SerializeField] private int maxPointsCount;
        [SerializeField] private int pointsRegeneration;
        private int _preTakenCurrentPoints;

        public UnityEvent OnActionPointsEnded;

        private void RegeneratePoints()
        {
            currentPoints += pointsRegeneration;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
        }

        #region GetSet

        public int MaxPointsCount
        {
            get => maxPointsCount;
        }
        
        public int CurrentActionPoints
        {
            get => currentPoints;
            set => currentPoints = value;
        }
        
        public int PreTakenCurrentPoints
        {
            get => _preTakenCurrentPoints;
            set => _preTakenCurrentPoints = value;
        }
        
        #endregion
        
        public int RegenActionPoints
        {
            get => pointsRegeneration;
        }

        public bool IsActionPointsEnough(int cost)
        {
            return currentPoints >= cost;
        }

        public void SpendPoints(int cost)
        {
            CalculateCost(ref currentPoints, cost);

            if (currentPoints <= 0)
                OnActionPointsEnded.Invoke();
        }
        
        public void PreSpendPoints(int preCost)
        {
            CalculateCost(ref _preTakenCurrentPoints, preCost);
        }

        private void CalculateCost(ref int points, int cost)
        {
            points -= cost;
        }
        
        public void ResetPreCostValue()
        {
            _preTakenCurrentPoints = currentPoints;
        }

        public bool Full => currentPoints >= maxPointsCount;

        public void Tick() => RegeneratePoints();
    }
}