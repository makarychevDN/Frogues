namespace FroguesFramework
{
    public interface IAbleToCost
    {
        public int GetActionPointsCost();
        public int GetBloodPointsCost();
        public int GetHealthCost();
        public bool IsResoursePointsEnough();
        public void SpendResourcePoints();
    }
}