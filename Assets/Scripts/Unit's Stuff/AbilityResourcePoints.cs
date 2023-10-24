using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilityResourcePoints : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization, IAbleToCalculateHashFunctionOfPrevisualisation
    {
        [SerializeField] private int currentPoints;
        [SerializeField] private int tempraryPoints;
        [SerializeField] private int maxPointsCount;
        [SerializeField] private int pointsRegeneration;
        private int _preTakenCurrentPoints;
        private int _preTakenTemporaryPoints;
        private bool _isEnemy;

        public UnityEvent OnPointsEnded;
        public UnityEvent OnPointsRegenerated;
        public UnityEvent OnPointsIncreased;
        public UnityEvent OnPickUpPoints;

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
            tempraryPoints = 0;
            _preTakenTemporaryPoints = tempraryPoints;
            OnPointsRegenerated.Invoke();
            OnPointsIncreased.Invoke();
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

        public int TemporaryPoints
        {
            get => tempraryPoints;
        }

        public int PreTakenCurrentPoints
        {
            get => _preTakenCurrentPoints;
        }

        public int PreTakenTemporaryPoints
        {
            get => _preTakenTemporaryPoints;
        }

        #endregion

        public int PointsRegeneration
        {
            get => pointsRegeneration;
        }

        public bool IsPointsEnough(int cost)
        {
            return currentPoints + tempraryPoints >= cost;
        }

        public void SetCurrentPoints(int value)
        {
            currentPoints = value;
        }

        public void SetTemporaryPoints(int value)
        {
            currentPoints = value;
        }

        public void IncreasePoints(int value)
        {
            currentPoints += value;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
            _preTakenCurrentPoints = currentPoints;
            OnPointsIncreased.Invoke();
        }

        public void PickupPoints(int value)
        {
            OnPickUpPoints.Invoke();
            IncreasePoints(value);
        }

        public void IncreaseTemporaryPoints(int value)
        {
            tempraryPoints += value;
            OnPointsIncreased.Invoke();
        }

        public void SpendPoints(int cost)
        {
            CalculateCost(ref currentPoints, ref tempraryPoints, cost);

            if (currentPoints <= 0)
                OnPointsEnded.Invoke();
        }
        
        public void PreSpendPoints(int preCost)
        {
            CalculateCost(ref _preTakenCurrentPoints, ref _preTakenTemporaryPoints, preCost);
        }

        private void CalculateCost(ref int points, ref int temporarypPoints, int cost)
        {
            int spendedTemporaryCost = Mathf.Clamp(cost, 0, temporarypPoints);
            cost -= spendedTemporaryCost;
            temporarypPoints -= spendedTemporaryCost;
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
            _preTakenTemporaryPoints = tempraryPoints;
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
            return maxPointsCount ^ currentPoints ^ tempraryPoints ^ _preTakenCurrentPoints ^ _preTakenTemporaryPoints;
        }
    }
}