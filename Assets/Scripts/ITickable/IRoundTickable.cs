namespace FroguesFramework
{
    public interface IRoundTickable
    {
        public void TickBeforePlayerTurn();
        
        public void TickBeforeEnemiesTurn();
    }
}