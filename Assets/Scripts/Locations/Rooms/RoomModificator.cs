using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class RoomModificator : MonoBehaviour, IAbleToHaveTheMainQuest
    {
        [Header("Reach The Cell Quest")]
        [SerializeField] private Cell questTargetCell;
        public UnityEvent OnPlayerReachedTheCell;

        private Room _myRoom;

        public UnityEvent GetMainQuestCompletedEvent() => OnPlayerReachedTheCell;

        public void Init(Room room)
        {
            _myRoom = room;

            questTargetCell.OnBecameFullByUnit.AddListener(ComleteQuestIfUnitIsPlayer);
        }

        public void ComleteQuestIfUnitIsPlayer(Unit unit)
        {
            if (!_myRoom.PlayableCharacters.Contains(unit))
                return;

            OnPlayerReachedTheCell.Invoke();
        }
    }
}