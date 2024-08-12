using UnityEngine.Events;

namespace FroguesFramework
{
    public interface IAbleToHaveTheMainQuest 
    {
        public UnityEvent GetMainQuestCompletedEvent();
    }
}