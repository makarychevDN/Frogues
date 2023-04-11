using UnityEngine;

namespace FroguesFramework
{
    public class UseAbilityOnTargetOrSkipTurnAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeField] private MonoBehaviour _useOnTargetAbility;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private AbleToSkipTurn _ableToSkipTurn;
        
        public void Act()
        {
            if (((IAbleToUseOnUnit)_useOnTargetAbility).PossibleToUseOnUnit(target) && _actionPoints.IsActionPointsEnough(((IAbility)_useOnTargetAbility).GetCost()))
            {
                ((IAbleToUseOnUnit)_useOnTargetAbility).UseOnUnit(target);
                return;
            }
            
            _ableToSkipTurn.AutoSkip();
        }

        public void Init()
        {
            _unit = GetComponentInParent<Unit>();
            _actionPoints = _unit.ActionPoints;
            _ableToSkipTurn = _unit.AbleToSkipTurn;
            ((IAbility)_useOnTargetAbility).Init(_unit);

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}