using UnityEngine;

namespace FroguesFramework
{
    public class CheckSomeoneIsActRightNow : MonoBehaviour
    {
        private void Update()
        {
            print(CurrentlyActiveObjects.SomethingIsActNow);
        }
    }
}