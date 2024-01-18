namespace FroguesFramework
{
    public interface IAbleToDealDamage
    {
        public int GetDefaultDamage();
        public DamageType GetDamageType();
        public int CalculateDamage();
    }
}