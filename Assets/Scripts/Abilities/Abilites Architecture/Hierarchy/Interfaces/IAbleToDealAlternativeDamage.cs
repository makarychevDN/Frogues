namespace FroguesFramework
{
    public interface IAbleToDealAlternativeDamage
    {
        public int GetDefaultAlternativeDamage();
        public DamageType GetAlternativeDamageType();
        public int CalculateAlternativeDamage();
    }
}