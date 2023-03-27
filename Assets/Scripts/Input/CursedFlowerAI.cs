using UnityEngine;

namespace FroguesFramework
{
    public class CursedFlowerAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private SpawnAndMoveUnitAbility _attackAbility;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private AbleToSkipTurn _ableToSkipTurn;

        public void Act()
        {
            if (_actionPoints.IsActionPointsEnough(_attackAbility.GetCost()) && _attackAbility.PossibleToUse())
            {
                _attackAbility.Use();
                return;
            }

            _ableToSkipTurn.AutoSkip();
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            _actionPoints = _unit.ActionPoints;
            _ableToSkipTurn = _unit.AbleToSkipTurn;
            _attackAbility.Init(_unit);
        }
    }
}