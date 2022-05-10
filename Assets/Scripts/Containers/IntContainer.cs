namespace FroguesFramework
{
    public class IntContainer : Container<int>
    {
        public void Increase(int value) => Content += value;
        public void Decrease(int value) => Content -= value;
        public void Increase() => Content++;
        public void Decrease() => Content--;
    }
}