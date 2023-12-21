namespace FroguesFramework
{
    public interface IAbleToApplyDefenceModificator
    {
        public int GetDefenceModificatorValue();
        public int GetdeltaOfDefenceValueForEachTurn();
        public int GetTimeToEndOfDefenceEffect();
        public bool GetDefenceEffectIsConstantly();
    }
}