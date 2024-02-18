namespace FroguesFramework
{
    public interface IInitializeableAbility
    {
        public void Init(Unit unit);
        public void SetOwner(Unit unit);
        public void UnInit();
    }
}