namespace FroguesFramework
{
    public interface IAbility
    {
        public void VisualizePreUse();

        public void Use();
        
        public void ApplyEffect();

        public void Init(Unit unit);
        
    }
}