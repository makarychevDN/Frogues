using UnityEngine;

namespace FroguesFramework
{
    public class RestartScene : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }

        public void Restart()
        {
            CurrentlyActiveObjects.Clear();
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}