using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class TrailBetweenRoomButtons : MonoBehaviour
    {
        [SerializeField] private RoomButton firstRoomButton;
        [SerializeField] private RoomButton secondRoomButton;

        public RoomButton FirstRoomButton => firstRoomButton;
        public RoomButton SecondRoomButton => secondRoomButton;

        public void Init(RoomButton firstRoomButton, RoomButton secondRoomButton)
        {
            this.firstRoomButton = firstRoomButton;
            this.secondRoomButton = secondRoomButton;
        }

        [ContextMenu("Update Transform")]
        public void UpdateTransform()
        {
            Vector3 trailVector = secondRoomButton.transform.localPosition - firstRoomButton.transform.localPosition;
            transform.localPosition = trailVector * 0.5f + firstRoomButton.transform.localPosition;
            (transform as RectTransform).sizeDelta = new Vector2(((int)trailVector.magnitude - 30) / 8 * 8 , 4);
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
    }
}