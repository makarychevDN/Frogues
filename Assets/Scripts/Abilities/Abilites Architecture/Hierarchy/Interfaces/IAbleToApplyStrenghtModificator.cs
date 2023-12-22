namespace FroguesFramework
{
    public interface IAbleToApplyStrenghtModificator
    {
        public int GetStrenghtModificatorValue();
        public int GetDeltaOfStrenghtValueForEachTurn();
        public int GetTimeToEndOfStrenghtEffect();
        public bool GetStrenghtEffectIsConstantly();
    }
}