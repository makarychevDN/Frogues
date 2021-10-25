using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBoxValueFromIntContainer : MonoBehaviour
{
    [SerializeField] private IntContainer intContainer;
    [SerializeField] private TextMeshProUGUI textField;

    void Update()
    {
        textField.text = intContainer.Content.ToString();
    }
}
