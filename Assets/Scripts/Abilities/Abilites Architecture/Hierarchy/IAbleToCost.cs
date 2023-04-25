namespace FroguesFramework
{
    public interface IAbleToCost
    {
        public int GetCost();
        public bool IsActionPointsEnough();
        public void SpendActionPoints();
    }
}