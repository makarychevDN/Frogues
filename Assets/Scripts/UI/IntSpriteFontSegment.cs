using UnityEngine;

public class IntSpriteFontSegment : MonoBehaviour
{
    [SerializeField] private GameObject minusSimbol;
    [SerializeField] private GameObject[] firstDigits;
    [SerializeField] private GameObject[] secondDigits;
    [SerializeField] private GameObject secondDigitsParents;

    public void SetValue(int value)
    {
        secondDigitsParents.SetActive(false);

        var valueIsNegative = value < 0;
        minusSimbol.SetActive(valueIsNegative);

        var firstDigit = Mathf.Abs(value % 10);
        for (int i = 0; i < firstDigits.Length; i++)
        {
            firstDigits[i].SetActive(firstDigit == i);
        }

        if (Mathf.Abs(value) < 10)
            return;

        secondDigitsParents.SetActive(true);
        var secondDigit = Mathf.Abs(value / 10);
        for (int i = 0; i < secondDigits.Length; i++)
        {
            secondDigits[i].SetActive(secondDigit == i);
        }
    }
}