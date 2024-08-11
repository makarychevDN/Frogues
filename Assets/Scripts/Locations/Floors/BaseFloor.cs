using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class BaseFloor : MonoBehaviour
    {
        [SerializeField] protected List<Unit> playableCharacters;
        [SerializeField] protected Room currentRoom;
        [SerializeField] protected List<RoomButton> roomButtons;
        [SerializeField] private bool roomsAreAbleToBeRevisited;

        void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            foreach (RoomButton roomButton in roomButtons)
            {
                roomButton.Button.onClick.AddListener(() => OpenTheRoom(roomButton.GetRoom(), playableCharacters));
            }
        }

        public void OpenTheRoom(Room room, List<Unit> playableCharacters)
        {
            if (currentRoom != null)
            {
                currentRoom.UnInit(playableCharacters);
                currentRoom.gameObject.SetActive(false);
            }

            room.gameObject.SetActive(true);
            room.Init(playableCharacters);
            currentRoom = room;
        }

        [ContextMenu("create trails between room buttons")]
        public void SpawnTrailsBetweenRoomButtons()
        {

        }
    }
}