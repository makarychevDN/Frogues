using UnityEngine;

namespace FroguesFramework
{
    public class UseAbilityOrSkipTurnAI : MonoBehaviour, IAbleToAct
    {
        [SerializeField] private Unit target;
        [SerializeReference] private MonoBehaviour _attackAbility;
        private Unit _unit;
        private ActionPoints _actionPoints;
        private AbleToSkipTurn _ableToSkipTurn;
        
        public void Act()
        {
            if (((IAbleToUseOnTarget)_attackAbility).PossibleToUseOnTarget(target) && _actionPoints.IsActionPointsEnough(_attackAbility.GetCost()))
            {
                ((IAbleToUseOnTarget)_attackAbility).UseOnTarget(target);
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

            if (target == null)
                target = EntryPoint.Instance.MetaPlayer;
        }
    }
}