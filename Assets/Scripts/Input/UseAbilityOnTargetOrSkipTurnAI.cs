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
            if (((IAbleToUseOnTarget)_useOnTargetAbility).PossibleToUseOnTarget(target) && _actionPoints.IsActionPointsEnough(((IAbility)_useOnTargetAbility).GetCost()))
            {
                ((IAbleToUseOnTarget)_useOnTargetAbility).UseOnTarget(target);
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