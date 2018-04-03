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

        private void Start()
        {
            ArenaGrid = GameObject.FindGameObjectWithTag(Utilities.Constants.TagArena);
        }

        public bool isInitiated = false;
        
        private bool isVertical = true;

        private int GetMaxHeight()
        {
            return ArenaGridPiecesCache.Max(ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY);
        }

        private int GetMaxWidth()
        {
            return ArenaGridPiecesCache.Max(ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX);
        }

        private List<GameObject> FindVerticalLane(int coordX, int coordY)
        {
            return ArenaGridPiecesCache
                .Where(
                    ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX == coordX &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY <= GetMaxHeight() &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY >= 0
                ).ToList();
        }

        private List<GameObject> FindHorizontalLane(int coordX, int coordY)
        {
            return ArenaGridPiecesCache
                .Where(ap =>
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY == coordY &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX <= GetMaxWidth() &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX >= 0
                ).ToList();
        }

        internal List<GameObject> GetVerticalEdges()
        {
            return ArenaGridPiecesCache
                .Where(ap =>
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX == 0 ||
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX == GetMaxWidth()
                ).ToList();
        }

        internal List<GameObject> GetHorizontalEdges()
        {
            return ArenaGridPiecesCache
                .Where(ap =>
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY == 0 ||
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY == GetMaxHeight()
                ).ToList();
        }

        internal void HandleUserClick(int coordX, int coordY)
        {
            if (!isInitiated)
            {
                return;
            }

            if (isVertical)
            {
                foreach (var piece in FindVerticalLane(coordX, coordY))
                {
                    CreateWallPiece(piece);
                    Destroy(piece);
                    ArenaGridPiecesCache.Remove(piece);
                }
            }
            else
            {
                foreach (var piece in FindHorizontalLane(coordX, coordY))
                {
                    CreateWallPiece(piece);
                    Destroy(piece);
                    ArenaGridPiecesCache.Remove(piece);
                }
            }
        }

        private void CreateWallPiece(GameObject piece)
        {
            Vector3 wallSpawnPosition = new Vector3(
                piece.transform.position.x,
                piece.transform.position.y + (piece.GetComponentInChildren<Transform>().localScale.y * 2),
                piece.transform.position.z
            );
            GameObject createdWallPiece = Instantiate(ArenaWallPiece,
                wallSpawnPosition,
                ArenaWallPiece.transform.rotation,
                ArenaGrid.transform);

            Debug.Log(createdWallPiece.tag);

            ArenaWallPiecesCache.Add(createdWallPiece);
        }

        internal Utilities.WallType GetWallType(int coordX, int coordY)
        {
            if(coordX == 0)
            {
                return Utilities.WallType.Left;
            }
            else if(coordX == GetMaxWidth())
            {
                return Utilities.WallType.Right;
            }
            else if(coordY == 0)
            {
                return Utilities.WallType.Bottom;
            }
            else if(coordY == GetMaxHeight())
            {
                return Utilities.WallType.Top;
            }
            else
            {
                Debug.LogWarning($"Unknown wall X:{coordX} Y:{coordY}");
                return Utilities.WallType.Top;
            }
        }

        public void SwitchDirection(bool isVertical)
        {
            this.isVertical = isVertical;
        }

        public ArenaGridPiece GetRandomArenaGridPiece()
        {

            return ArenaGridPiecesCache[UnityEngine.Random.Range(0, ArenaGridPiecesCache.Count)]
                .GetComponentInChildren<ArenaGridPiece>();              
        }

        public ArenaGridPiece GetRandomArenaGridPieceWithinWalls()
        {
            List<GameObject> verticalEdges = GetVerticalEdges();
            List<GameObject> horizontalEdges = GetHorizontalEdges();

            List<GameObject> piecesWithinWalls = new List<GameObject>();

            ArenaGridPiecesCache.ForEach(p =>
            {
                if(!verticalEdges.Contains(p) && !horizontalEdges.Contains(p))
                {
                    piecesWithinWalls.Add(p);
                }
            });
            return piecesWithinWalls[UnityEngine.Random.Range(0, piecesWithinWalls.Count)]
                .GetComponentInChildren<ArenaGridPiece>();
        }

        public void InitComplete()
        {
            isInitiated = true;
        }
    }
}

