using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class ReachTheTargetCellRoomModificator : RoomModificator, IAbleToHaveTheMainQuest
    {
        [SerializeField] private Cell questTargetCell;
        private Room _myRoom;

        public UnityEvent OnPlayerReachedTheCell;

        public override void Init(Room room)
        {
            _myRoom = room;

            questTargetCell.OnBecameFullByUnit.AddListener(ComleteQuestIfUnitIsPlayer);
        }

        public UnityEvent GetMainQuestCompletedEvent() => OnPlayerReachedTheCell;

        public void ComleteQuestIfUnitIsPlayer(Unit unit)
        {
            if (!_myRoom.PlayableCharacters.Contains(unit))
                return;

            OnPlayerReachedTheCell.Invoke();
        }
    }
}