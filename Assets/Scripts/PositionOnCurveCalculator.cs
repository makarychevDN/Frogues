using FroguesFramework;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public static class PositionOnCurveCalculator
{
    public static Vector3 Calculate(Vector3 startPosition, Vector3 endPosition, AnimationCurve curve, float lerpValue, float maxHeight)
    {
        Vector3 lerpedPosition = Vector3.Lerp(startPosition, endPosition, lerpValue);
        lerpedPosition += Vector3.up * (curve.Evaluate(lerpValue) * maxHeight);

        return lerpedPosition;
    }
    
    public static Vector3 Calculate(Cell startCell, Cell endCell, AnimationCurve curve, float lerpValue, float maxHeight)
    {
        return Calculate(startCell.transform.position, endCell.transform.position, curve, lerpValue, maxHeight);
    }
}
