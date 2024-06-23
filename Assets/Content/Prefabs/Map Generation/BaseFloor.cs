using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class BaseFloor : MonoBehaviour
    {
        [SerializeField] private Unit player;
        [SerializeField] private Room currentRoom;
        [SerializeField] private Room room;
        [SerializeField] private List<Room> roomsOnTheLevel;
        [SerializeField] private bool needToGenerateRooms;
        [SerializeField] private FloorGenerator floorGenerator;
        [SerializeField] private Transform map;
        [SerializeField] protected List<RoomButton> roomButtons;
        [SerializeField] private bool roomsAreAbleToBeRevisited;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            foreach (RoomButton roomButton in roomButtons)
            {
                roomButton.Button.onClick.AddListener(() => OpenTheRoom(roomButton.GetRoom(), new List<Unit> { player }));
            }

            if(needToGenerateRooms)
            {
                GenerateRooms();
            }
        }

        public void GenerateRooms()
        {
            floorGenerator.GenerateFloor();
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