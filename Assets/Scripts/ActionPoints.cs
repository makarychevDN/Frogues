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
        private bool _enemy;

        public UnityEvent OnActionPointsEnded;

        public void Init(Unit unit)
        {
            _enemy = unit.Enemy;
        }

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
        }
        
        public int PreTakenCurrentPoints
        {
            get => _preTakenCurrentPoints;
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

        public void TickBeforePlayerTurn()
        { 
            if (_enemy)
                RegeneratePoints();
        }
        
        public void TickBeforeEnemiesTurn()
        {
            if (!_enemy)
                RegeneratePoints();
        }
    }
}