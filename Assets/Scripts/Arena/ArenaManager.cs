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
                .Where(
                    ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY == coordY &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX <= GetMaxWidth() &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX >= 0
                ).ToList();
        }

        internal void HandleUserClick(int coordX, int coordY)
        {
            bool isVertical = false;
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
    }
}

