using UnityEngine;

namespace FroguesFramework
{
public class BigSlimeAI : MonoBehaviour, IAbleToAct
{
    [SerializeField] private Unit target;
    [SerializeField] private RatKnifeAbility _knifeAbility;
    [SerializeField] private SplitOnSmallSlimesAbility _splitAbility;
    private Unit _unit;
    private MovementAbility _movementAbility;
    private ActionPoints _actionPoints;
    private AbleToSkipTurn _ableToSkipTurn;
    
    public void Act()
    {
        if (_unit.Health.Full)
            ActionOnFullHealth();

        else
            ActionOnNotFullHealth();
    }

    private void ActionOnFullHealth()
    {
        if (_knifeAbility.PossibleToUseOnTarget(target) && _actionPoints.IsActionPointsEnough(_knifeAbility.GetCost()))
        {
            _knifeAbility.UseOnTarget(target);
            return;
        }
        
        var path = EntryPoint.Instance.PathFinder.FindWayExcludeLastCell(_unit.CurrentCell, target.CurrentCell, false, false, false);

        if (path == null || path.Count == 0 || !_actionPoints.IsActionPointsEnough(_movementAbility.GetCost()))
        {
            _ableToSkipTurn.AutoSkip();
            return;
        }
        
        _movementAbility.TargetCell = path[0];
        _movementAbility.Use();
    }
    
    private void ActionOnNotFullHealth()
    {
        _splitAbility.Use();
    }

    public void Init()
    {
        _unit = GetComponentInParent<Unit>();
        _movementAbility = _unit.MovementAbility;
        _actionPoints = _unit.ActionPoints;
        _ableToSkipTurn = _unit.AbleToSkipTurn;
        _knifeAbility.Init(_unit);

        if (target == null)
            target = EntryPoint.Instance.MetaPlayer;
    }
}
}