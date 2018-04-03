using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public static class ArenaUtilities
    {

        private static int GetMaxHeight(List<GameObject> arenaPieces)
        {
            return arenaPieces.Max(ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY);
        }

        private static int GetMaxWidth(List<GameObject> arenaPieces)
        {
            return arenaPieces.Max(ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX);
        }

        private static int GetMinHeight(List<GameObject> arenaPieces)
        {
            return arenaPieces.Min(ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY);
        }

        private static int GetMinWidth(List<GameObject> arenaPieces)
        {
            return arenaPieces.Min(ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX);
        }

        internal static List<GameObject> FindVerticalLane(int coordX, int coordY, List<GameObject> arenaPieces)
        {
            return arenaPieces
                .Where(
                    ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX == coordX &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY <= GetMaxHeight(arenaPieces) &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY >= GetMinHeight(arenaPieces)
                ).ToList();
        }

        internal static List<GameObject> FindHorizontalLane(int coordX, int coordY, List<GameObject> arenaPieces)
        {
            return arenaPieces
                .Where(ap =>
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY == coordY &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX <= GetMaxWidth(arenaPieces) &&
                    ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX >= GetMinWidth(arenaPieces)
                ).ToList();
        }

        internal static List<GameObject> GetVerticalEdges(List<GameObject> arenaPieces)
        {
            return arenaPieces
                .Where(ap =>
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX == GetMinWidth(arenaPieces) ||
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX == GetMaxWidth(arenaPieces)
                ).ToList();
        }

        internal static List<GameObject> GetHorizontalEdges(List<GameObject> arenaPieces)
        {
            return arenaPieces
                .Where(ap =>
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY == GetMinHeight(arenaPieces) ||
                ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY == GetMaxHeight(arenaPieces)
                ).ToList();
        }

        internal static Utilities.WallType GetWallType(int coordX, int coordY, List<GameObject> arenaPieces)
        {
            if (coordX == GetMinWidth(arenaPieces))
            {
                return Utilities.WallType.Left;
            }
            else if (coordX == GetMaxWidth(arenaPieces))
            {
                return Utilities.WallType.Right;
            }
            else if (coordY == GetMinWidth(arenaPieces))
            {
                return Utilities.WallType.Bottom;
            }
            else if (coordY == GetMaxHeight(arenaPieces))
            {
                return Utilities.WallType.Top;
            }
            else
            {
                Debug.LogWarning($"Unknown wall X:{coordX} Y:{coordY}");
                return Utilities.WallType.Top;
            }
        }

        internal static List<GameObject> FindWallPiecesOutsideOfVerticalLane(int coordX,
            bool leftSide, List<GameObject> arenaWallPieces)
        {
            if (leftSide)
            {
                return arenaWallPieces
                    .Where(
                        wp => wp.GetComponentInChildren<WallHandler>().CoordinateX < coordX
                    )
                    .ToList();
            }
            else
            {
                return arenaWallPieces
                    .Where(
                        wp => wp.GetComponentInChildren<WallHandler>().CoordinateX > coordX
                    )
                    .ToList();
            }
        }

        internal static List<GameObject> FindPiecesOutsideOfVerticalLane(int coordX, bool leftSide,
            List<GameObject> arenaPieces)
        {
            if (leftSide)
            {
                return arenaPieces
                    .Where(
                        ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX < coordX
                    )
                    .ToList();
            }
            else
            {
                return arenaPieces
                    .Where(
                        ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateX > coordX
                    )
                    .ToList();
            }
        }

        internal static List<GameObject> FindPiecesOutsideOfHorizontalLane(int coordY, bool downSide,
    List<GameObject> arenaPieces)
        {
            if (downSide)
            {
                return arenaPieces
                    .Where(
                        ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY < coordY
                    )
                    .ToList();
            }
            else
            {
                return arenaPieces
                    .Where(
                        ap => ap.GetComponentInChildren<ArenaGridPiece>().CoordinateY > coordY
                    )
                    .ToList();
            }
        }

        internal static List<GameObject> FindWallPiecesOutsideOfHorizontalLane(int coordY, bool downSide,
List<GameObject> arenaWallPieces)
        {
            if (downSide)
            {
                return arenaWallPieces
                    .Where(
                        ap => ap.GetComponentInChildren<WallHandler>().CoordinateY < coordY
                    )
                    .ToList();
            }
            else
            {
                return arenaWallPieces
                    .Where(
                        ap => ap.GetComponentInChildren<WallHandler>().CoordinateY > coordY
                    )
                    .ToList();
            }
        }

        public static ArenaGridPiece GetRandomArenaGridPiece(List<GameObject> arenaPieces)
        {

            return arenaPieces[Random.Range(0, arenaPieces.Count)]
                .GetComponentInChildren<ArenaGridPiece>();
        }

        public static ArenaGridPiece GetRandomArenaGridPieceWithinWalls(List<GameObject> arenaPieces)
        {
            List<GameObject> verticalEdges = GetVerticalEdges(arenaPieces);
            List<GameObject> horizontalEdges = GetHorizontalEdges(arenaPieces);

            List<GameObject> piecesWithinWalls = new List<GameObject>();

            arenaPieces.ForEach(p =>
            {
                if (!verticalEdges.Contains(p) && !horizontalEdges.Contains(p))
                {
                    piecesWithinWalls.Add(p);
                }
            });
            return piecesWithinWalls[Random.Range(0, piecesWithinWalls.Count)]
                .GetComponentInChildren<ArenaGridPiece>();
        }



    }
}
