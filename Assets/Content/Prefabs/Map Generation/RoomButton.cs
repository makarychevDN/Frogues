using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] private bool ableToClick;
        private Room _room;
        public Button Button => button;
        public UnityEvent OnRoomButtonSelected;

        public bool AbleToClick
        {
            get => ableToClick;
            set
            {
                ableToClick = value;
                button.interactable = value;
            }
        }

        public Room GetRoom()
        {
            if (_room == null)
            {
                _room = Instantiate(roomPrefab);
            }

            OnRoomButtonSelected.Invoke();
            return _room;
        }

        public void Init(Room room)
        {
            roomPrefab = room;
        }

        public void SetButtonIsInteractable(bool value) => button.interactable = !value;
    }
}