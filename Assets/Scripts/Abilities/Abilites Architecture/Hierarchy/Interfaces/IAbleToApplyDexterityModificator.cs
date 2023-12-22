namespace FroguesFramework
{
    public interface IAbleToApplyDexterityModificator 
    {
        public int GetDexterityModificatorValue();
        public int GetDeltaOfDexterityValueForEachTurn();
        public int GetTimeToEndOfDexterityEffect();
        public bool GetDexterityEffectIsConstantly();
    }
}