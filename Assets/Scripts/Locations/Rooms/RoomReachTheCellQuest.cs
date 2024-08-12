using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class RoomReachTheCellQuest : Room, IAbleToHaveTheMainQuest
    {
        [Header("Reach The Cell Quest")]
        [SerializeField] private Cell questTargetCell;
        public UnityEvent OnPlayerReachedTheCell;

        public UnityEvent GetMainQuestCompletedEvent() => OnPlayerReachedTheCell;

        public override void Init(List<Unit> playableCharacters)
        {
            base.Init(playableCharacters);

            questTargetCell.OnBecameFullByUnit.AddListener(ComleteQuestIfUnitIsPlayer);
        }

        public void ComleteQuestIfUnitIsPlayer(Unit unit)
        {
            if (!playableCharacters.Contains(unit))
                return;

            OnPlayerReachedTheCell.Invoke();
        }
    }
}