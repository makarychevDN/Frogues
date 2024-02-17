namespace FroguesFramework
{
    public interface IInitializeableAbility
    {
        public void Init(Unit unit, bool unitBecomesAnOwner = true);
        public void Init(Unit unit);
        public void UnInit();
    }
}