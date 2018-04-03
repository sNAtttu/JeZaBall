using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class WallCreationScript : MonoBehaviour
    {
        [Tooltip("Time between wall pieces")]
        public float WallPieceCreateDelay = 0.1f;

        [Tooltip("Arena wall piece which are created to the edges")]
        public GameObject ArenaWallPiece;

        private GameObject ArenaGrid;

        private void Start()
        {
            ArenaGrid = GameObject.FindGameObjectWithTag(Utilities.Constants.TagArena);
        }

        public void RemovePiecesHorizontal(int coordX, int coordY, bool removeDownSide, 
            List<GameObject> arenaPiecesCache, List<GameObject> wallPiecesCache)
        {
            List<GameObject> piecesToBeCreated = ArenaUtilities.FindHorizontalLane(coordX, coordY, arenaPiecesCache);
            List<GameObject> piecesLeft = piecesToBeCreated.Where(p => p.GetComponentInChildren<ArenaGridPiece>().CoordinateX < coordX).Reverse().ToList();
            List<GameObject> pieceClickedAndRight = piecesToBeCreated.Except(piecesLeft).ToList();

            if (piecesLeft.Count > pieceClickedAndRight.Count)
            {
                StartCoroutine(CreateHorizontalWallPieces(piecesLeft, removeDownSide, coordY, true, 
                    arenaPiecesCache, wallPiecesCache));
                StartCoroutine(CreateHorizontalWallPieces(pieceClickedAndRight, removeDownSide, coordY, 
                    false, arenaPiecesCache, wallPiecesCache));
            }
            else
            {
                StartCoroutine(CreateHorizontalWallPieces(piecesLeft, removeDownSide, coordY, false, 
                    arenaPiecesCache, wallPiecesCache));
                StartCoroutine(CreateHorizontalWallPieces(pieceClickedAndRight, removeDownSide, coordY, 
                    true, arenaPiecesCache, wallPiecesCache));
            }
        }

        public void RemovePiecesVertical(int coordX, int coordY, bool removeLeftSide,
            List<GameObject> arenaPiecesCache, List<GameObject> wallPiecesCache)
        {
            List<GameObject> piecesToBeCreated = ArenaUtilities.FindVerticalLane(coordX, coordY, arenaPiecesCache);
            List<GameObject> piecesTopOfClick = piecesToBeCreated.Where(p => p.GetComponentInChildren<ArenaGridPiece>().CoordinateY > coordY).ToList();
            List<GameObject> pieceClickedAndUnderIt = piecesToBeCreated.Except(piecesTopOfClick)
                .Reverse()
                .ToList();

            if (piecesTopOfClick.Count > pieceClickedAndUnderIt.Count)
            {
                StartCoroutine(CreateVerticalWallPieces(piecesTopOfClick, removeLeftSide, coordX, true, arenaPiecesCache, wallPiecesCache));
                StartCoroutine(CreateVerticalWallPieces(pieceClickedAndUnderIt, removeLeftSide, coordX, false, arenaPiecesCache, wallPiecesCache));
            }
            else
            {
                StartCoroutine(CreateVerticalWallPieces(piecesTopOfClick, removeLeftSide, coordX, false, arenaPiecesCache, wallPiecesCache));
                StartCoroutine(CreateVerticalWallPieces(pieceClickedAndUnderIt, removeLeftSide, coordX, true, arenaPiecesCache, wallPiecesCache));
            }

        }

        private void RemoveUnnecessaryVerticalPieces(int coordX, bool removeLeftSide, 
            List<GameObject> arenaPiecesCache, List<GameObject> wallPiecesCache)
        {
            foreach (var pieceToDestroy in ArenaUtilities.FindPiecesOutsideOfVerticalLane(coordX,
                removeLeftSide, arenaPiecesCache))
            {
                Destroy(pieceToDestroy);
                arenaPiecesCache.Remove(pieceToDestroy);
            }
            foreach (var wallToDestroy in ArenaUtilities.FindWallPiecesOutsideOfVerticalLane(coordX,
                removeLeftSide, wallPiecesCache))
            {
                Destroy(wallToDestroy);
                wallPiecesCache.Remove(wallToDestroy);
            }
        }
        private void RemoveUnnecessaryHorizontalPieces(int coordY, bool removeDownSide, 
            List<GameObject> arenaPiecesCache, List<GameObject> wallPiecesCache)
        {
            foreach (var gridPiece in ArenaUtilities.FindPiecesOutsideOfHorizontalLane(
                coordY, removeDownSide, arenaPiecesCache))
            {
                Destroy(gridPiece);
                arenaPiecesCache.Remove(gridPiece);
            }
            foreach (var wallToDestroy in ArenaUtilities.FindWallPiecesOutsideOfHorizontalLane(coordY,
                removeDownSide, wallPiecesCache))
            {
                Destroy(wallToDestroy);
                wallPiecesCache.Remove(wallToDestroy);
            }
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

        private GameObject CreateHorizontalWallPiece(GameObject piece, bool isNewDownWall)
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

            return createdWallPiece;
        }
        private GameObject CreateVerticalWallPiece(GameObject piece, bool isNewLeftWall)
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

            return createdWallPiece;
        }

        IEnumerator CreateHorizontalWallPieces(List<GameObject> piecesToBeCreated, bool removeDownSide,
    int coordY, bool removeAfterSuccess, List<GameObject> arenaPiecesCache,
    List<GameObject> wallPiecesCache)
        {
            List<GameObject> objectsCreated = new List<GameObject>();
            foreach (var piece in piecesToBeCreated)
            {
                CreateHorizontalWallPiece(piece, removeDownSide);
                yield return new WaitForSeconds(WallPieceCreateDelay);
            }
            arenaPiecesCache.AddRange(objectsCreated);
            if (removeAfterSuccess)
            {
                RemoveUnnecessaryHorizontalPieces(coordY, removeDownSide, arenaPiecesCache, wallPiecesCache);
            }
        }

        IEnumerator CreateVerticalWallPieces(List<GameObject> piecesToBeCreated, bool removeLeftSide,
            int coordX, bool removeAfterSuccess, List<GameObject> arenaPiecesCache,
            List<GameObject> wallPiecesCache)
        {
            List<GameObject> objectsCreated = new List<GameObject>();
            foreach (var piece in piecesToBeCreated)
            {
                CreateVerticalWallPiece(piece, removeLeftSide);
                yield return new WaitForSeconds(WallPieceCreateDelay);
            }
            arenaPiecesCache.AddRange(objectsCreated);
            if (removeAfterSuccess)
            {
                RemoveUnnecessaryVerticalPieces(coordX, removeLeftSide, arenaPiecesCache, wallPiecesCache);
            }
        }
    }
}