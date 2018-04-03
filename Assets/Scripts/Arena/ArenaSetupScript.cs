using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class ArenaSetupScript : MonoBehaviour
    {
        [Tooltip("Width of the arena")]
        public int ArenaWidth = 10;

        [Tooltip("Height of the arena")]
        public int ArenaHeight = 10;

        [Tooltip("Pieces which are used in creation")]
        public GameObject ArenaGridPiece;

        [Tooltip("Arena wall piece which are created to the edges")]
        public GameObject ArenaWallPiece;

        private ArenaManager arenaManager;

        // Use this for initialization
        void Start()
        {
            arenaManager = GetComponent<ArenaManager>();
            CreateArenaGrid(ArenaWidth, ArenaHeight);
            CreateArenaWalls(arenaManager.ArenaGridPiecesCache);
        }

        private void CreateArenaGrid(int width, int height)
        {
            Vector3 spawnPosition = new Vector3();
            List<GameObject> createdObjects = new List<GameObject>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject createdPiece = Instantiate(ArenaGridPiece,
                        spawnPosition,
                        ArenaGridPiece.transform.rotation,
                        gameObject.transform);

                    createdPiece.GetComponentInChildren<ArenaGridPiece>().CoordinateX = j;
                    createdPiece.GetComponentInChildren<ArenaGridPiece>().CoordinateY = i;
                    createdObjects.Add(createdPiece);
                    spawnPosition.x += (ArenaGridPiece.GetComponentInChildren<Transform>().localScale.x * 2);
                }
                spawnPosition.z += (ArenaGridPiece.GetComponentInChildren<Transform>().localScale.z * 2);
                spawnPosition.x = 0;
            }
            arenaManager.ArenaGridPiecesCache = createdObjects;
        }

        private void CreateArenaWalls(List<GameObject> arenaGridPieces)
        {
            List<GameObject> createdWallPieces = new List<GameObject>();
            foreach (var horizontalEdgePiece in ArenaUtilities.GetHorizontalEdges(arenaGridPieces))
            {
                CreateWallPiece(createdWallPieces, horizontalEdgePiece, arenaGridPieces);
            }

            foreach (var verticalEdgePiece in ArenaUtilities.GetVerticalEdges(arenaGridPieces))
            {
                CreateWallPiece(createdWallPieces, verticalEdgePiece, arenaGridPieces);
            }
            arenaManager.ArenaWallPiecesCache = createdWallPieces;
        }

        private void CreateWallPiece(
            List<GameObject> createdWallPieces,
            GameObject verticalEdgePiece, 
            List<GameObject> piecesCache)
        {
            Vector3 spawnPosition = new Vector3(
                verticalEdgePiece.transform.position.x,
                verticalEdgePiece.transform.position.y + (ArenaGridPiece.transform.localScale.y * 2),
                verticalEdgePiece.transform.position.z
                );

            GameObject createdWallPiece = Instantiate(ArenaWallPiece,
                spawnPosition,
                ArenaWallPiece.transform.rotation,
                gameObject.transform);

            WallHandler handler = createdWallPiece.GetComponentInChildren<WallHandler>();

            int x = verticalEdgePiece.GetComponentInChildren<ArenaGridPiece>().CoordinateX;
            int y = verticalEdgePiece.GetComponentInChildren<ArenaGridPiece>().CoordinateY;

            handler.CoordinateX = x;
            handler.CoordinateY = y;
            handler.WallType = ArenaUtilities.GetWallType(x, y, piecesCache);

            createdWallPieces.Add(createdWallPiece);
        }
    }
}