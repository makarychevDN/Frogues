using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private IntContainer maxHp;
    [SerializeField] private IntContainer currentHp;
    [SerializeField] private TextMeshProUGUI textField;

    public void Update()
    {
        slider.maxValue = maxHp.Content;
        slider.value = currentHp.Content;

        if(textField != null)
            textField.text = currentHp.Content.ToString();
    }
} 
