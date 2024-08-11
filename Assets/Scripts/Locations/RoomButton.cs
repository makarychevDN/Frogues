using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FroguesFramework
{
    [RequireComponent(typeof(Button))]
    public class RoomButton : MonoBehaviour
    {
        [SerializeField] private Room roomPrefab;
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        [SerializeField] private bool ableToClick;
        [SerializeField] private Sprite RoomVisitedSprite;

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

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
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

        public void Init(Room roomPrefab)
        {
            this.roomPrefab = roomPrefab;
        }

        public void SetSpriteAsRoomIsAvailable() => image.sprite = roomPrefab.AvailableSprite;

        public void SetSpriteAsRoomIsUnavailable() => image.sprite = roomPrefab.UnavailableSprite;
    }
}