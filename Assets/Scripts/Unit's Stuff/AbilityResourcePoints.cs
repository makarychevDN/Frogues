using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilityResourcePoints : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization, IAbleToCalculateHashFunctionOfPrevisualisation
    {
        [SerializeField] private int currentPoints;
        [SerializeField] private int maxPointsCount;
        [SerializeField] private int pointsRegeneration;
        private int _preTakenCurrentPoints;
        private bool _isEnemy;

        public UnityEvent OnPointsEnded;
        public UnityEvent OnPointsRegenerated;

        public void Init(Unit unit)
        {
            _isEnemy = unit.IsEnemy;
            AddMySelfToEntryPoint();
        }

        private void RegeneratePoints()
        {
            currentPoints += pointsRegeneration;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
            _preTakenCurrentPoints = currentPoints;
            OnPointsRegenerated.Invoke();
        }

        #region GetSet

        public int MaxPointsCount
        {
            get => maxPointsCount;
        }
        
        public int CurrentPoints
        {
            get => currentPoints;
        }
        
        public int PreTakenCurrentPoints
        {
            get => _preTakenCurrentPoints;
        }
        
        #endregion
        
        public int PointsRegeneration
        {
            get => pointsRegeneration;
        }

        public bool IsPointsEnough(int cost)
        {
            return currentPoints >= cost;
        }

        public void SetPoints(int value)
        {
            currentPoints = value;
        }

        public void IncreasePoints(int value)
        {
            currentPoints += value;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
        }

        public void SpendPoints(int cost)
        {
            CalculateCost(ref currentPoints, cost);

            if (currentPoints <= 0)
                OnPointsEnded.Invoke();
        }
        
        public void PreSpendPoints(int preCost)
        {
            CalculateCost(ref _preTakenCurrentPoints, preCost);
        }

        private void CalculateCost(ref int points, int cost)
        {
            points -= cost;
        }

        public bool Full => currentPoints >= maxPointsCount;

        public void TickAfterEnemiesTurn()
        { 
            if (_isEnemy)
                RegeneratePoints();
        }
        
        public void TickAfterPlayerTurn()
        {
            if (!_isEnemy)
                RegeneratePoints();
        }

        public void DisablePreVisualization()
        {
            _preTakenCurrentPoints = currentPoints;
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public int CalculateHashFunctionOfPrevisualisation()
        {
            return maxPointsCount ^ currentPoints ^ _preTakenCurrentPoints;
        }
    }
}