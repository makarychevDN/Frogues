using UnityEngine;

namespace FroguesFramework
{
    public class NextRoomStarter : MonoBehaviour
    {
        public void StartNextRoom() => EntryPoint.Instance.StartNextRoom();
    }
}