using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class TrainingInFirstRoom : BaseTrainingModificator
    {
        [SerializeField] private Vector3 cameraStartPosition = new Vector3(9f, 0.9f, 6.0f);
        private int _tooBigPriceForMovement = 999;

        public override void Init()
        {
            EntryPoint.Instance.MetaPlayer.MovementAbility.IncreaseActionPointsCost(_tooBigPriceForMovement);
            (EntryPoint.Instance.MetaPlayer.ActionsInput as MonoBehaviour).enabled = false;
            Camera.main.transform.position = cameraStartPosition;
            EntryPoint.Instance.CameraController.OnCameraReseted.AddListener(EnableInput);
            EntryPoint.Instance.CameraController.OnCameraRotated.AddListener(EnableMovement);
            EntryPoint.Instance.MetaPlayer.GetComponentsInChildren<Collider>().ToList().ForEach(collider => collider.enabled = false);
            EntryPoint.Instance.EndTurnButton.SetActive(false);
        }

        private void EnableInput()
        {
            (EntryPoint.Instance.MetaPlayer.ActionsInput as MonoBehaviour).enabled = true;
            EntryPoint.Instance.CameraController.OnCameraReseted.RemoveListener(EnableInput);
        }

        private void EnableMovement()
        {
            EntryPoint.Instance.MetaPlayer.MovementAbility.IncreaseActionPointsCost(-_tooBigPriceForMovement - 1);
            EntryPoint.Instance.CameraController.OnCameraRotated.RemoveListener(EnableMovement);
        }
    }
}