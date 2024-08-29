using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class SurviveForNTurnsRoomModificator : RoomModificator, IRoundTickable, IAbleToHaveTheMainQuest
    {
        [SerializeField] private int requiredQuantityOfTurns;
        private int turnsCounter;

        public UnityEvent OnPlayerSurvived;

        public override void Init(Room room)
        {
            turnsCounter = 0;

        }

        public void TickAfterEnemiesTurn()
        {
            turnsCounter++;

            if(turnsCounter == requiredQuantityOfTurns)
            {
                OnPlayerSurvived.Invoke();
            }
        }

        public void TickAfterPlayerTurn() { }

        public UnityEvent GetMainQuestCompletedEvent() => OnPlayerSurvived;

        private void SpawnEnemies()
        {

        }
    }
}