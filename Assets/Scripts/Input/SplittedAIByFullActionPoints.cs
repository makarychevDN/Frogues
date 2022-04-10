using UnityEngine;

namespace FroguesFramework
{
    public class SplittedAIByFullActionPoints : BaseInput
    {
        [SerializeField] private BaseInput inputOnFullActionPoints;
        [SerializeField] private BaseInput inputOnNonFullActionPoints;
        [SerializeField] private ActionPoints actionPoints;
        [SerializeField] private AbleToSkipTurn ableToSkipTurn;
        private BaseInput _currentInput;

        private void Start()
        {
            inputOnFullActionPoints.OnInputDone.AddListener(EndInput);
            inputOnNonFullActionPoints.OnInputDone.AddListener(EndInput);
            actionPoints.OnActionPointsEnded.AddListener(EndInput);
        }

        public override void Act()
        {
            if (_currentInput == null)
            {
                _currentInput = actionPoints.Full ? inputOnFullActionPoints : inputOnNonFullActionPoints;
            }

            _currentInput.Act();
        }

        public void EndInput()
        {
            _currentInput = null;
            ableToSkipTurn.AutoSkip();
        }
    }
}
