namespace FroguesFramework
{
    public class KillItSelfAfterEnemiesTurn : PassiveAbility, IRoundTickable
    {
        public void TickAfterEnemiesTurn()
        {
            _owner.AbleToDie.DieWithoutAnimation();
        }

        public void TickAfterPlayerTurn() { }
    }
}