using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    [RequireComponent(typeof(Button))]
    public class RoomButton : MonoBehaviour
    {
        [SerializeField] private Room roomPrefab;
        [SerializeField] private Button button;
        [SerializeField] protected List<RoomButton> neighbors;

        public Button Button => button;

        public void Init(Room room)
        {
            roomPrefab = room;
        }
    }
}