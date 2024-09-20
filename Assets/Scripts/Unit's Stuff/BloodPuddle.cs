namespace FroguesFramework
{
    public class BloodPuddle : Unit
    {
        public override void Init(Room room)
        {
            base.Init(room);
            CurrentRoom.AddBloodPuddle(this);
        }

        public void Absorb()
        {
            AbleToDie.DieWithoutAnimation();
            CurrentRoom.RemoveBloodPuddle(this);
        }
    }
}