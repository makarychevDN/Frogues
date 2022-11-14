namespace FroguesFramework
{
    public interface IAbleToUseOnTarget 
    {
        public bool PossibleToUseOnTarget(Unit target);
        
        public void UseOnTarget(Unit target);
    }
}