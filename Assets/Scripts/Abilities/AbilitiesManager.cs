using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilitiesManager : MonoBehaviour
    {
        [SerializeField] private List<IAbility> abilities = new();

        public void AddAbility(IAbility ability)
        {
            abilities.Add(ability);
        }

        public void RemoveAbility(IAbility ability)
        {
            abilities.Remove(ability);
        }
    }
}