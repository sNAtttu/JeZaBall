using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class WallHandler : MonoBehaviour
    {
        public int CoordinateX;
        public int CoordinateY;

        public Utilities.WallType WallType;
        public Utilities.WallState WallState;

        private WallCreationScript wallCreation;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == Utilities.Constants.TagBall)
            {
                if(WallState == Utilities.WallState.Creating)
                {
                    if(wallCreation == null)
                    {
                        wallCreation = FindObjectOfType<WallCreationScript>();
                    }
                    wallCreation.SetWallHittedOnCreation(gameObject);
                }
                collision.transform.GetComponentInParent<BallMovement>().ChangeMovementDirection(WallType);
            }
        }
    }
}