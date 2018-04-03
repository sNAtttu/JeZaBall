using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class ArenaManager : MonoBehaviour
    {
        [Tooltip("Arena wall piece which are created to the edges")]
        public GameObject ArenaWallPiece;

        public List<GameObject> ArenaGridPiecesCache;
        public List<GameObject> ArenaWallPiecesCache;

        private GameObject ArenaGrid;
        private List<BallManager> ballManagers;

        private void Start()
        {
            ballManagers = new List<BallManager>();
            ArenaGrid = GameObject.FindGameObjectWithTag(Utilities.Constants.TagArena);
        }

        public bool isInitiated = false;
        
        private bool isVertical = true;

        private bool IsBallLeftOfTheLane(int coordX)
        {
            return ballManagers.Any(b => b.CurrentCoordX < coordX);
        }

        private bool IsBallRightOfTheLane(int coordX)
        {
            return ballManagers.Any(b => b.CurrentCoordX > coordX);
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
                bool ballIsOnRightSide = true;
                foreach (var pieceToDestroy in ArenaUtilities.FindPiecesOutsideOfVerticalLane(coordX, 
                    ballIsOnRightSide, ArenaGridPiecesCache))
                {
                    Destroy(pieceToDestroy);
                    ArenaGridPiecesCache.Remove(pieceToDestroy);
                }
                foreach (var wallToDestroy in ArenaUtilities.FindWallPiecesOutsideOfVerticalLane(coordX, ballIsOnRightSide, ArenaWallPiecesCache))
                {
                    Destroy(wallToDestroy);
                    ArenaWallPiecesCache.Remove(wallToDestroy);
                }
                foreach (var piece in ArenaUtilities.FindVerticalLane(coordX, coordY, ArenaGridPiecesCache))
                {
                    CreateVerticalWallPiece(piece, ballIsOnRightSide);
                }
            }
            else
            {

            }
        }

        private void CreateVerticalWallPiece(GameObject piece, bool isNewLeftWall)
        {

            int x = piece.GetComponentInChildren<ArenaGridPiece>().CoordinateX;
            int y = piece.GetComponentInChildren<ArenaGridPiece>().CoordinateY;

            Vector3 wallSpawnPosition = new Vector3(
                piece.transform.position.x,
                piece.transform.position.y + (piece.GetComponentInChildren<Transform>().localScale.y * 2),
                piece.transform.position.z
            );
            GameObject createdWallPiece = Instantiate(ArenaWallPiece,
                wallSpawnPosition,
                ArenaWallPiece.transform.rotation,
                ArenaGrid.transform);

            WallHandler handler = createdWallPiece.GetComponentInChildren<WallHandler>();

            handler.CoordinateX = x;
            handler.CoordinateY = y;

            handler.WallType = isNewLeftWall ? Utilities.WallType.Left : Utilities.WallType.Right;

            ArenaWallPiecesCache.Add(createdWallPiece);
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

