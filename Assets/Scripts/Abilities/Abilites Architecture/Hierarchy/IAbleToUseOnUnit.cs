namespace FroguesFramework
{
    public interface IAbleToUseOnUnit 
    {
        public void PrepareToUsing(Unit target);
        public bool PossibleToUseOnUnit(Unit target);      
        public void UseOnUnit(Unit target);
        public void VisualizePreUseOnUnit(Unit target);
    }
}