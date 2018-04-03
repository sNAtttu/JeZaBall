using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ball;

namespace Arena
{
    public class ArenaManager : MonoBehaviour
    {
        public List<GameObject> ArenaGridPiecesCache;
        public List<GameObject> ArenaWallPiecesCache;

        private List<BallManager> ballManagers;
        private WallCreationScript wallCreation;

        public bool isInitiated = false;

        private bool isVertical = true;

        private void Start()
        {
            wallCreation = GetComponent<WallCreationScript>();
            ballManagers = new List<BallManager>();
            
        }

        internal void HandleUserClick(int coordX, int coordY)
        {
            if (!isInitiated)
            {
                return;
            }

            if(ballManagers.Count == 0)
            {
                ballManagers = FindObjectsOfType<BallManager>().ToList();
            }

            if (isVertical)
            {
                bool leftSideCanBeDestroyed = !BallUtilities.IsBallLeftOfTheLane(coordX, ballManagers);
                bool rightSideCanBeDestroyed = !BallUtilities.IsBallRightOfTheLane(coordX, ballManagers);
                if (leftSideCanBeDestroyed)
                {
                    wallCreation.RemovePiecesVertical(coordX, coordY, true, ArenaGridPiecesCache, ArenaWallPiecesCache);
                }
                if (rightSideCanBeDestroyed)
                {
                    wallCreation.RemovePiecesVertical(coordX, coordY, false, ArenaGridPiecesCache, ArenaWallPiecesCache);
                }
            }
            else
            {
                bool topFromTheLaneCanBeDestroyed = !BallUtilities.IsBallUpFromTheLane(coordY, ballManagers);
                bool downFromTheLaneCanBeDestroyed = !BallUtilities.IsBallDownFromTheLane(coordY, ballManagers);

                if (downFromTheLaneCanBeDestroyed)
                {
                    wallCreation.RemovePiecesHorizontal(coordX, coordY, true, 
                        ArenaGridPiecesCache, ArenaWallPiecesCache);
                }
                if (topFromTheLaneCanBeDestroyed)
                {
                    wallCreation.RemovePiecesHorizontal(coordX, coordY, false, 
                        ArenaGridPiecesCache, ArenaWallPiecesCache);
                }

            }
        }

        public void SwitchDirection(bool isVertical)
        {
            this.isVertical = isVertical;
        }

        public void InitComplete()
        {
            isInitiated = true;
        }
    }
}

