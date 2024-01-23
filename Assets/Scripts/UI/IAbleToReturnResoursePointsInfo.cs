namespace FroguesFramework
{
    public interface IAbleToReturnResoursePointsInfo
    {
        public int GetCurrentResourceCount();
        public int GetMaxResourceCount();
        public int GetRegenOfResource();
        public int GetTemporaryResourceCount();
    }
}