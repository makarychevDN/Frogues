namespace FroguesFramework
{
    public interface IAbleToUseOnUnit 
    {
        public bool PossibleToUseOnUnit(Unit target);      
        public void UseOnUnit(Unit target);
        public void VisualizePreUseOnUnit(Unit target);
    }
}