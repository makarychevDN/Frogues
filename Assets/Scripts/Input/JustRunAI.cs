namespace FroguesFramework
{
    public class JustRunAI : AlternatesRunFromTargetAndDoSomethingAI
    {
        protected override void TryToDoSomething() => TryToRunFromTarget();
    }
}