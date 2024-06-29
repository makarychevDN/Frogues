using UnityEngine;

namespace FroguesFramework
{
    public class HubFloor : BaseFloor
    {
        [SerializeField] private Room theFirstRoom;

        public override void Init()
        {
            OpenTheRoom(theFirstRoom, playableCharacters);
            base.Init();
        }
    }
}