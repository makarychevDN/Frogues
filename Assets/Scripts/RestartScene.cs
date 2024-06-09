using UnityEngine;

namespace FroguesFramework
{
    public class RestartScene : MonoBehaviour
    {
        public void Restart()
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}