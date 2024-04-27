using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class WallsEnabler : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private List<Pillar> pillars;
        [SerializeField] private List<AnimatedWall> walls;
        private float _angleDelta = 90;

        private void Update()
        {
            EnableWalls();
            EnablePillars();
        }

        private void EnableWalls()
        {
            foreach (var wall in walls)
            {
                wall.Showed = Vector3.Angle(Camera.main.transform.position - wall.ThikcnessPointsOnTheFrontSide.position, wall.transform.forward) < _angleDelta;
            }
        }

        private void EnablePillars()
        {
            foreach (var pillar in pillars)
            {
                pillar.itSelf.Showed = pillar.NeedToShow;
            }
        }
    }

    [Serializable]
    public struct Pillar
    {
        public bool requiresAllActiveNeighbors;
        public AnimatedWall itSelf;
        public List<AnimatedWall> neighborWalls;

        public bool NeedToShow => requiresAllActiveNeighbors ? neighborWalls.All(wall => wall.Showed) : neighborWalls.Any(wall => wall.Showed);
    }
}