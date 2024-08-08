using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class TrailBetweenRoomButtons : MonoBehaviour
    {
        [SerializeField] private RoomButton firstRoomButton;
        [SerializeField] private RoomButton secondRoomButton;
        [SerializeField] private Image image;
        [SerializeField] private Sprite updatedSprite;

        public RoomButton FirstRoomButton => firstRoomButton;
        public RoomButton SecondRoomButton => secondRoomButton;

        public void Init(RoomButton firstRoomButton, RoomButton secondRoomButton)
        {
            this.firstRoomButton = firstRoomButton;
            this.secondRoomButton = secondRoomButton;

            firstRoomButton.Button.onClick.AddListener(() => UpdateSprite(updatedSprite));
            secondRoomButton.Button.onClick.AddListener(() => UpdateSprite(updatedSprite));
        }

        [ContextMenu("Update Transform")]
        public void UpdateTransform()
        {
            Vector3 trailVector = secondRoomButton.transform.localPosition - firstRoomButton.transform.localPosition;
            transform.localPosition = trailVector * 0.5f + firstRoomButton.transform.localPosition;
            (transform as RectTransform).sizeDelta = new Vector2(((int)trailVector.magnitude - 28) / 8 * 8 , 4);
            transform.right = secondRoomButton.transform.position - transform.position;
        }

        public bool EqualToOtherLine(TrailBetweenRoomButtons otherLine)
        {
            return otherLine.firstRoomButton == firstRoomButton && otherLine.secondRoomButton == secondRoomButton ||
                otherLine.firstRoomButton == secondRoomButton && otherLine.secondRoomButton == firstRoomButton;
        }

        public bool EqualToOtherLineInTheList(List<TrailBetweenRoomButtons> otherLines)
        {
            return otherLines.Where(otherLine => otherLine != this).Any(otherLine => EqualToOtherLine(otherLine));
        }

        public void UpdateSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}