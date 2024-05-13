using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class CutSceneInTheTavernModificator : BaseTrainingModificator
    {
        [SerializeField] private Vector3 cameraStartPosition = new Vector3(7.64f, 1.13f, 4.0f);
        [SerializeField] private Transform movementTrailObject;

        public override void Init()
        {
            CurrentlyActiveObjects.Add(this);
            Camera.main.transform.parent = movementTrailObject;
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
            FindObjectOfType<PlayerInput>().enabled = false;
            FindObjectsOfType<Unit>().ToList().ForEach(unit => unit.gameObject.SetActive(false));
            FindObjectsOfType<Canvas>().ToList().ForEach(canvas => canvas.gameObject.SetActive(false));
        }
    }
}