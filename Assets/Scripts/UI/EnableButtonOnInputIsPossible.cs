using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButtonOnInputIsPossible : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Button button;

    private void Update()
    {
        button.interactable = playerInput.InputIsPossible;
    }
}
