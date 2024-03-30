using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class FakeQueue : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;

        void Update()
        {
            if (CurrentlyActiveObjects.SomethingIsActNow)
                return;

            playerInput.Act();
        }
    }
}