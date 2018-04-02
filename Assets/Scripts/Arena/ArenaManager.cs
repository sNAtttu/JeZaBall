using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class ArenaManager : MonoBehaviour
    {
        public List<GameObject> ArenaGridPiecesCache;
        public List<GameObject> ArenaWallPiecesCache;

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
                    Destroy(piece);
                    ArenaGridPiecesCache.Remove(piece);
                }
            }
            else
            {
                foreach (var piece in FindHorizontalLane(coordX, coordY))
                {
                    Destroy(piece);
                    ArenaGridPiecesCache.Remove(piece);
                }
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
        public void InitComplete()
        {
            isInitiated = true;
        }
    }
}

