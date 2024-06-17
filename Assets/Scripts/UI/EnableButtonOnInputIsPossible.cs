using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class EnableButtonOnInputIsPossible : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Button button;

        public void Init(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }

        private void Update()
        {
            button.interactable = playerInput.InputIsPossible;
        }
    }
}