using UnityEngine;

namespace FroguesFramework
{
    public class SetCursorOnStart : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursorTexture;

        void Start()
        {
            Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
