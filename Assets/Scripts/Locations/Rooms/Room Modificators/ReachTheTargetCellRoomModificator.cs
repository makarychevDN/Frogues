using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class ReachTheTargetCellRoomModificator : RoomModificator, IAbleToHaveTheMainQuest
    {
        [SerializeField] private Cell questTargetCell;
        [SerializeField] private int experienceForTheMainQuest;
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
            _myRoom.PlayableCharacters[0].ExperienceContainer.AddExpirience(experienceForTheMainQuest);
        }
    }
}