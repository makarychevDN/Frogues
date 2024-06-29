using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    [RequireComponent(typeof(Button))]
    public class RoomButton : MonoBehaviour
    {
        public FloorGeneratorNode FloorGeneratorNode { get; set; }
        [SerializeField] private Room roomPrefab;
        [SerializeField] private Button button;
        [SerializeField] private GameObject roomIsCompletedIndicator;
        [SerializeField] private List<RoomButton> neighbors;
        [SerializeField] private bool ableToClick;
        private Room _room;

        public Button Button => button;

        public bool AbleToClick
        {
            get => ableToClick;
            set 
            { 
                ableToClick = value;
                button.interactable = value;
            }
        }

        private void Awake()
        {
            //_room = Instantiate(roomPrefab);
            //_room.gameObject.SetActive(false);
            //_room.OnRoomWasEnabled.AddListener(SetButtonIsInteractable);
        }

        public Room GetRoom()
        {
            if (_room == null)
            {
                _room = Instantiate(roomPrefab);
            }

            return _room;
        }

        public void Init(Room room)
        {
            roomPrefab = room;
        }

        public void SetButtonIsInteractable(bool value) => button.interactable = !value;
    }
}