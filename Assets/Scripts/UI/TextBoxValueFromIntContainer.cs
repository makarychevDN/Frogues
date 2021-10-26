using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBoxValueFromIntContainer : MonoBehaviour
{
    [SerializeField] private IntContainer intContainer;
    [SerializeField] private IntContainer preContainer;
    [SerializeField] private TextMeshProUGUI textField;

    void Update()
    {
        textField.text = intContainer.Content.ToString();
        textField.color = Color.white;

        if (preContainer.Content != intContainer.Content)
        {
            textField.text = preContainer.Content.ToString();
            textField.color = Color.green;
        }
    }
}
