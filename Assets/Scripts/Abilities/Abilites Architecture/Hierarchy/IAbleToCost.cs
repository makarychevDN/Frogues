namespace FroguesFramework
{
    public interface IAbleToCost
    {
        public int GetActionPointsCost();
        public int GetBloodPointsCost();
        public bool IsResoursePointsEnough();
        public void SpendResourcePoints();
    }
}