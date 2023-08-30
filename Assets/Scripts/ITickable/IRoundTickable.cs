namespace FroguesFramework
{
    public interface IRoundTickable
    {
        public void TickAfterEnemiesTurn();
        
        public void TickAfterPlayerTurn();
    }
}