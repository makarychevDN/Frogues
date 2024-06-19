using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class TrailBetweenRoomButtons : MonoBehaviour
    {
        [SerializeField] private RoomButton firstRoomButton;
        [SerializeField] private RoomButton secondRoomButton;

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
            (transform as RectTransform).sizeDelta = new Vector2(trailVector.magnitude - 35, 4);
            transform.right = secondRoomButton.transform.position - transform.position;
        }
    }
}