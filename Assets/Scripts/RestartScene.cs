using UnityEngine;

namespace FroguesFramework
{
    public class RestartScene : MonoBehaviour
    {
        public void Restart()
        {
            CurrentlyActiveObjects.Clear();
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}