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
        [Tooltip("Time between wall pieces")]
        public float WallPieceCreateDelay = 0.2f;

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
                    RemovePiecesVertical(coordX, coordY, true);
                }
                if (rightSideCanBeDestroyed)
                {
                    RemovePiecesVertical(coordX, coordY, false);
                }
            }
            else
            {
                bool topFromTheLaneCanBeDestroyed = !BallUtilities.IsBallUpFromTheLane(coordY, ballManagers);
                bool downFromTheLaneCanBeDestroyed = !BallUtilities.IsBallDownFromTheLane(coordY, ballManagers);

                if (downFromTheLaneCanBeDestroyed)
                {
                    RemovePiecesHorizontal(coordX, coordY, true);
                }
                if (topFromTheLaneCanBeDestroyed)
                {
                    RemovePiecesHorizontal(coordX, coordY, false);
                }

            }
        }

        private void RemovePiecesHorizontal(int coordX, int coordY, bool removeDownSide)
        {
            List<GameObject> piecesToBeCreated = ArenaUtilities.FindHorizontalLane(coordX, coordY, ArenaGridPiecesCache);
            StartCoroutine(CreateHorizontalWallPieces(piecesToBeCreated, removeDownSide, coordY));
        }

        private void RemovePiecesVertical(int coordX, int coordY, bool removeLeftSide)
        {
            List<GameObject> piecesToBeCreated = ArenaUtilities.FindVerticalLane(coordX, coordY, ArenaGridPiecesCache);
            StartCoroutine(CreateVerticalWallPieces(piecesToBeCreated, removeLeftSide, coordX));
        }

        private static void GetNewPieceSpawnPosition(GameObject piece, out int x, out int y, out Vector3 wallSpawnPosition)
        {
            x = piece.GetComponentInChildren<ArenaGridPiece>().CoordinateX;
            y = piece.GetComponentInChildren<ArenaGridPiece>().CoordinateY;
            wallSpawnPosition = new Vector3(
                piece.transform.position.x,
                piece.transform.position.y + (piece.GetComponentInChildren<Transform>().localScale.y * 2),
                piece.transform.position.z
            );
        }

        public void SwitchDirection(bool isVertical)
        {
            this.isVertical = isVertical;
        }

        public void InitComplete()
        {
            isInitiated = true;
        }

        private void RemoveUnnecessaryVerticalPieces(int coordX, bool removeLeftSide)
        {
            foreach (var pieceToDestroy in ArenaUtilities.FindPiecesOutsideOfVerticalLane(coordX,
                removeLeftSide, ArenaGridPiecesCache))
            {
                Destroy(pieceToDestroy);
                ArenaGridPiecesCache.Remove(pieceToDestroy);
            }
            foreach (var wallToDestroy in ArenaUtilities.FindWallPiecesOutsideOfVerticalLane(coordX,
                removeLeftSide, ArenaWallPiecesCache))
            {
                Destroy(wallToDestroy);
                ArenaWallPiecesCache.Remove(wallToDestroy);
            }
        }
        private void RemoveUnnecessaryHorizontalPieces(int coordY, bool removeDownSide)
        {
            foreach (var gridPiece in ArenaUtilities.FindPiecesOutsideOfHorizontalLane(
                coordY, removeDownSide, ArenaGridPiecesCache))
            {
                Destroy(gridPiece);
                ArenaGridPiecesCache.Remove(gridPiece);
            }
            foreach (var wallToDestroy in ArenaUtilities.FindWallPiecesOutsideOfHorizontalLane(coordY,
                removeDownSide, ArenaWallPiecesCache))
            {
                Destroy(wallToDestroy);
                ArenaWallPiecesCache.Remove(wallToDestroy);
            }
        }
        private void CreateHorizontalWallPiece(GameObject piece, bool isNewDownWall)
        {
            int x, y;
            Vector3 wallSpawnPosition;
            GetNewPieceSpawnPosition(piece, out x, out y, out wallSpawnPosition);

            GameObject createdWallPiece = Instantiate(ArenaWallPiece,
                wallSpawnPosition,
                ArenaWallPiece.transform.rotation,
                ArenaGrid.transform);

            WallHandler handler = createdWallPiece.GetComponentInChildren<WallHandler>();

            handler.CoordinateX = x;
            handler.CoordinateY = y;

            handler.WallType = isNewDownWall ? Utilities.WallType.Bottom : Utilities.WallType.Top;

            ArenaWallPiecesCache.Add(createdWallPiece);

        }
        private void CreateVerticalWallPiece(GameObject piece, bool isNewLeftWall)
        {

            int x, y;
            Vector3 wallSpawnPosition;
            GetNewPieceSpawnPosition(piece, out x, out y, out wallSpawnPosition);

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

        IEnumerator CreateHorizontalWallPieces(List<GameObject> piecesToBeCreated, bool removeDownSide, int coordY)
        {
            foreach (var piece in piecesToBeCreated)
            {
                CreateHorizontalWallPiece(piece, removeDownSide);
                yield return new WaitForSeconds(WallPieceCreateDelay);
            }
            RemoveUnnecessaryHorizontalPieces(coordY, removeDownSide);
        }

        IEnumerator CreateVerticalWallPieces(List<GameObject> piecesToBeCreated, bool removeLeftSide, int coordX)
        {
            foreach (var piece in piecesToBeCreated)
            {
                CreateVerticalWallPiece(piece, removeLeftSide);
                yield return new WaitForSeconds(WallPieceCreateDelay);
            }
            RemoveUnnecessaryVerticalPieces(coordX, removeLeftSide);
        }

    }
}

