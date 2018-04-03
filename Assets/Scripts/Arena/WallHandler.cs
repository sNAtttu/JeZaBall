using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class WallHandler : MonoBehaviour
    {
        public int CoordinateX;
        public int CoordinateY;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Ball hit: {CoordinateX} {CoordinateY}");
        }
    }
}

