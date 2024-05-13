using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class CutSceneInTheTavernModificator : BaseTrainingModificator
    {
        [SerializeField] private Transform movementTrailObject;
        [SerializeField] private GameObject enemiesSprite;

        public override void Init()
        {
            Cursor.visible = false;
            enemiesSprite.SetActive(false);
            CurrentlyActiveObjects.Add(this);
            Camera.main.transform.parent = movementTrailObject;
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
            FindObjectOfType<PlayerInput>().enabled = false;
            FindObjectsOfType<Unit>().ToList().ForEach(unit => unit.gameObject.SetActive(false));
            FindObjectsOfType<Canvas>().ToList().ForEach(canvas => canvas.gameObject.SetActive(false));
            Invoke(nameof(TurnOnEnemies), 1.5f);
        }

        private void TurnOnEnemies()
        {
            enemiesSprite.SetActive(true);
        }
    }
}