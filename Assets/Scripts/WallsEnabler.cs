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
        [SerializeField] private List<GameObject> walls;
        [SerializeField] private float angleDelta = 55f;

        private void Update()
        {
            EnableWalls();
            EnablePillars();
        }

        private void EnableWalls()
        {
            foreach (var wall in walls)
            {
                wall.SetActive(Vector3.Angle(Camera.main.transform.forward, wall.transform.right) > angleDelta);
            }
        }

        private void EnablePillars()
        {
            foreach (var pillar in pillars)
            {
                pillar.itSelf.SetActive(pillar.neighborWalls.Any(wall => wall.activeInHierarchy));
            }
        }
    }

    [Serializable]
    public struct Pillar
    {
        public GameObject itSelf;
        public List<GameObject> neighborWalls;
    }
}