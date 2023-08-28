using UnityEngine;

namespace FroguesFramework
{
    public class StatsVisualizationSystem : MonoBehaviour
    {
        [SerializeField] private Stats stats;

        [SerializeField] private StatVisualizationSegment strenghtSegment;
        [SerializeField] private StatVisualizationSegment intelligenceSegment;
        [SerializeField] private StatVisualizationSegment dexteritySegment;
        [SerializeField] private StatVisualizationSegment defenceSegment;
        [SerializeField] private StatVisualizationSegment spikesSegment;

        void Update()
        {
            strenghtSegment.gameObject.SetActive(stats.Strenght != 0);
            strenghtSegment.SetValue(stats.Strenght);

            intelligenceSegment.gameObject.SetActive(stats.Intelegence != 0);
            intelligenceSegment.SetValue(stats.Intelegence);

            dexteritySegment.gameObject.SetActive(stats.Dexterity != 0);
            dexteritySegment.SetValue(stats.Dexterity);

            defenceSegment.gameObject.SetActive(stats.Defence != 0);
            defenceSegment.SetValue(stats.Defence);

            spikesSegment.gameObject.SetActive(stats.Spikes != 0);
            spikesSegment.SetValue(stats.Spikes);
        }
    }
}