using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButtonByScript : MonoBehaviour
{
    [SerializeField] private Button button;

    public void Click()
    {
        button.onClick.Invoke();
    }
}
