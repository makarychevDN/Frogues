using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class TriggerAllRoundTickable : MonoBehaviour
    {
        [SerializeField] private bool triggerBeforePlayer;
        public void RoundTick()
        {
            if(triggerBeforePlayer)
                FindObjectsOfType<MonoBehaviour>().OfType<IRoundTickable>().ToList().ForEach(x => x.TickBeforePlayerTurn());
            else
                FindObjectsOfType<MonoBehaviour>().OfType<IRoundTickable>().ToList().ForEach(x => x.TickBeforeEnemiesTurn());
        }
    }
}