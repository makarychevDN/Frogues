namespace FroguesFramework
{
    public class DeathOnMovementCompletedPassiveAbility : PassiveAbility
    {
        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Movable.OnMovementEnd.AddListener(_owner.AbleToDie.DieWithoutAnimation);
        }
    }
}