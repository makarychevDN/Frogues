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
        [SerializeField] private Image image;
        [SerializeField] private bool ableToClick;
        [SerializeField] private Sprite RoomVisitedSprite;

        private Room _room;
        private bool _visitedAlready;
        public Button Button => button;
        public UnityEvent OnRoomButtonSelected;

        public bool AbleToClick
        {
            get => ableToClick;
            set
            {
                if (_visitedAlready)
                {
                    ableToClick = false;
                    button.interactable = false;
                }
                else
                {
                    ableToClick = value;
                    button.interactable = value;
                }


                if (_visitedAlready)
                {
                    image.sprite = RoomVisitedSprite;
                }
                else
                {
                    image.sprite = value ? roomPrefab.AvailableSprite : roomPrefab.UnavailableSprite;
                }
            }
        }

        public Room GetRoom()
        {
            if(_room == null)
            {
                _room = Instantiate(roomPrefab);
            }

            OnRoomButtonSelected.Invoke();
            return _room;
        }

        public void TurnOnVisitedAlreadyMode()
        {
            _visitedAlready = true;
            AbleToClick = false;
        }

        public void Init(Room roomPrefab)
        {
            this.roomPrefab = roomPrefab;
        }

        public void SetButtonIsInteractable(bool value) => button.interactable = !value;
    }
}