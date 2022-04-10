using System.Text;
using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class DescriptionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI SomeTMProText;
        [SerializeField] private UnitsDescription description;

        private void Start()
        {
            StringBuilder sb = new StringBuilder();
            //description.descriptionTags.ForEach(sb.Append("{"<color=green> blablabla </color>"}"));

            SomeTMProText.SetText(
                $"hello {$"<color=#{ColorUtility.ToHtmlStringRGB(description.descriptionTags[0].textColor)}> {description.descriptionTags[0].Text} </color>"}");
            //SomeTMProText.

            //{ColorUtility.ToHtmlStringRGB(description.descriptionTags[0].textColor)}
        }
    }
}