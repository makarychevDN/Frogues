using FroguesFramework;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Unit player;
        [SerializeField] private Room room;
        [SerializeField] private List<Room> roomsOnTheLevel;
        [SerializeField] private bool needToGenerateRooms;
        [SerializeField] private FloorGenerator floorGenerator;

        void Start()
        {
            GenerateRooms();
            OpenTheRoom(room);
        }

        public void Init()
        {
            if(needToGenerateRooms)
            {
                GenerateRooms();
            }


        }

        public void GenerateRooms()
        {
            floorGenerator.GenerateFloor();
        }

        public void OpenTheRoom(Room room)
        {
            room.gameObject.SetActive(true);
            room.Init(new List<Unit> { player });
        }
    }
}