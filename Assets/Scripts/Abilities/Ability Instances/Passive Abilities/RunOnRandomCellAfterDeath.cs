namespace FroguesFramework
{
    public class RunOnRandomCellAfterDeath : PassiveAbility
    {
        public override void Init(Unit unit)
        {
            base.Init(unit);
            _owner.Health.OnEscapedFromDeath.AddListener(MoveToRandomCell);
        }

        public override void UnInit()
        {
            _owner.Health.OnEscapedFromDeath.RemoveListener(MoveToRandomCell);
            base.UnInit();
        }

        private void MoveToRandomCell()
        {
            var targetCell = CellsTaker.TakeAllEmptyCells().GetRandomElement();

            if (targetCell == null)
                return;

            _owner.Movable.Move(targetCell);
        }
    }
}